using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO.Ports;

namespace LCDMessageBoxController
{
    public partial class LCDMessageBoxController : ServiceBase
    {
        StockPrice stockSpy = new StockPrice("SPY");
        StockPrice stockMSFT = new StockPrice("MSFT");
        bool isSending = false;
        int waitingOnEcho = 0;
        bool hasRunOnce = false;
        object loopLock = new object();
        public System.Timers.Timer thisTimer;
        private string TRANSEIVERPORT;
        private int BAUDRATE = 115200;
        SerialPort serialPort;
        string outgoingMsg = "";
        const string PRINTSCR = "PRINTSCR";
        const int ROWS = 4;
        int lineNumber = -1;
        string incomingData = "";
        DateTime timeToRestart = DateTime.Now.AddMinutes(1);
        private bool isLoggingVerbose;

        public LCDMessageBoxController()
        {
            InitializeComponent();
        }

        public void Start()
        {
            this.OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            MoveToNextLine();
            isLoggingVerbose = Properties.Settings.Default.LoggingVerbose;
            TRANSEIVERPORT = Properties.Settings.Default.TRANSEIVERPORT ?? "COM3";
            serialPort = new SerialPort(TRANSEIVERPORT, BAUDRATE);
            serialPort.Open();
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            thisTimer = new System.Timers.Timer();
            thisTimer.Enabled = true;
            thisTimer.Interval = 2000;
            thisTimer.AutoReset = true;
            thisTimer.Elapsed += new System.Timers.ElapsedEventHandler(thisTimer_Elapsed);
            thisTimer.Start();
        }

        void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (MessageRecieved())
            {
                LogEvent("Message was recieved properly", EventLogEntryType.Information);
                MoveToNextLine();
                waitingOnEcho = 0;
            }
        }

        private void thisTimer_Elapsed(object sender, EventArgs e)
        {
            if (!ShouldRun()) { return; }

            if (isSending || waitingOnEcho-- > 0) { return; }

            lock (loopLock)
            {
                isSending = true;
                thisTimer.Stop();

                incomingData = "";
                waitingOnEcho = 10;
                SendSerialMessage( outgoingMsg, lineNumber);

                isSending = false;
                thisTimer.Start();
            }
        }

        private void MoveToNextLine()
        {
            if ((lineNumber >= ROWS) || (timeToRestart < DateTime.Now))
            {
                timeToRestart = DateTime.Now.AddSeconds(60);
                lineNumber = -1;
            }
            outgoingMsg = GetLineMessage(++lineNumber);
            LogEvent("moving to next line " + lineNumber.ToString());
        }

        private string GetLineMessage(int lineNumber)
        {
            switch (lineNumber)
            {
                case 0:
                    return string.Format("{0} = {1}", stockSpy.TickerSymbol, stockSpy.GetPrice());
                case 1:
                    return string.Format("{0} = {1}", stockMSFT.TickerSymbol, stockMSFT.GetPrice());
                case 2:
                    return string.Format("updated {0}", DateTime.Now.ToShortTimeString());
                case 3:
                    return "Don't be Stupid....";
                case 4:
                    return "PRINTSCR";
            }
            return "";
        }

        private bool ShouldRun()
        {
            if (!hasRunOnce) { return true; }

            if (
                (DateTime.Now.Hour > 16)
                ||
                (DateTime.Now.Hour == 16 & DateTime.Now.Minute > 15)
                ||
                (DateTime.Now.Hour < 9)
                ||
                (DateTime.Now.Hour == 9 & DateTime.Now.Minute < 30)
                ||
                (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                )
            {
                return false;
            }

            return true;
        }

        protected override void OnStop()
        {
        }

        private bool MessageRecieved()
        {
            try
            {
                LogEvent("Incoming bytes = " + serialPort.BytesToRead.ToString());

                if (serialPort.BytesToRead > 0)
                {
                    incomingData += serialPort.ReadExisting();
                    string[] incomingMsgs = incomingData.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string incomingMsg in incomingMsgs)
                    {
                        LogEvent("Data found = " + incomingMsg);
                        int lineNbr = 0;
                        if (incomingMsg.IndexOf(',') <= 0 )
                        {
                            LogEvent("No comma");
                            continue;
                        }
                        string lineNumberStr = incomingMsg.Substring(0, incomingMsg.IndexOf(','));
                        if (!int.TryParse(lineNumberStr, out lineNbr))
                        {
                            LogEvent("Found no line number");
                            continue;
                        }
                        string msgStr = incomingMsg.Substring(incomingMsg.IndexOf(',') + 1, incomingMsg.Length - lineNumberStr.Length - 1);
                        if (msgStr != outgoingMsg)
                        {
                            LogEvent(string.Format("Compare {0}\nTo str: {1}", incomingMsg, outgoingMsg));
                            continue;
                        }
                        waitingOnEcho = 0;
                        return true;
                    }
                    incomingData = incomingMsgs[incomingMsgs.Length-1];
                }
                return false;
            }
            catch (Exception ex)
            {
                LogEvent("Message was not recieved properly: " + ex.Message, EventLogEntryType.Warning);
            }
            return false;
        }

        private void SendSerialMessage(string outputMsg, int lineNumber)
        {
            try
            {
                serialPort.Write(new byte[] { 6 }, 0, 1); //ACK
                serialPort.Write(lineNumber.ToString());
                serialPort.Write(",");
                serialPort.Write(outputMsg + "\n");
                LogEvent("Message was sent properly");
            }
            catch (Exception ex)
            {
                LogEvent("Message was not sent properly: " + ex.Message, EventLogEntryType.Warning);
            }
        }

        public void LogEvent(string message)
        {
            LogEvent(message, EventLogEntryType.Information);
        }

        public void LogEvent(string message, EventLogEntryType type)
        {
            if (this.isLoggingVerbose || type != EventLogEntryType.Information)
            {
            EventLog.WriteEntry(message, type);
            }
        }
    }
}
