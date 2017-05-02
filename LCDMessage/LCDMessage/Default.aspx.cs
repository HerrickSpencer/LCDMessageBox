using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LCDMessage
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //txtLCDMessage.Attributes.Add("onKeyPress", "javascript:return KeyPressed();");
            lblUser.Text = System.Web.HttpContext.Current.User.Identity.Name.ToString();
            messageLogArea.Text = GetLogMessages();
            txtMessagePreview.Style.Add("overflow", "hidden");
        }

        private string GetLogMessages()
        {
            string returnMessages = string.Empty;

            returnMessages += "<table border=1><tr><th>User</th><th>Message</th><th>PostDate</th></tr>";

            using (DAL.LCDMessageDBEntities db = new DAL.LCDMessageDBEntities())
            {
                var messages = (
                    from p in db.MessageLogs
                    orderby p.messageID descending
                    select p ).Take(20);
                foreach (var item in messages)
                {
                    returnMessages += "<tr>";
                    returnMessages += "<td>" + item.username + "</td>";
                    string msg = item.message;
                    if (msg.Length > 1000)
                    {
                        msg = msg.Substring(0, 1000) + "....";
                    }
                    msg = FormatDisplayString(msg, "</br>");
                    returnMessages += "<td nowrap='nowrap'>" + msg + "</td>";
                    returnMessages += "<td>" + item.messageDate + "</td>";
                    returnMessages += "</tr>";
                }
            }
            returnMessages += "</table>";
            return returnMessages;
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            string outputMsg = FormatString(txtLCDMessage.Text);
            if (outputMsg == "!!!EXCEPTION!!!")
            {
                return;
            }
            txtMessagePreview.Text = FormatDisplayString(outputMsg, Environment.NewLine);
            //LogMessage(outputMsg);
            SendSerialMessage(outputMsg);
            messageLogArea.Text = GetLogMessages();
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            string outputMsg = FormatString(txtLCDMessage.Text);
            txtMessagePreview.Text = FormatDisplayString(outputMsg, Environment.NewLine);
        }

        private void SendSerialMessage(string outputMsg)
        {
            try
            {
                SerialPort sp3 = new SerialPort("COM3", 115200);
                sp3.Open();
                sp3.Write(outputMsg);
                sp3.Close();
            }
            catch (Exception)
            {
                lblStatus.Text = "Message was not sent properly";
                return;
            }

            lblStatus.Text = "Message sent";
        }


        private string FormatString(string input)
        {
            string rtnStr = string.Empty;

            try
            {
                input = HttpUtility.HtmlEncode(input);
            }
            catch (HttpRequestValidationException)
            {
                lblStatus.Text = "DO NOT PUT HTML IN MESSAGE!!!";
                return "!!!EXCEPTION!!!";
            }

            for (int row = 0; (row * 20) < input.Length; row++)
            {
                for (int i = 0; i < 20; i++)
                {
                    int chrPos = (row * 20) + i;
                    if (chrPos >= input.Length)
                    {
                        break;
                    }

                    string chr = input.Substring(chrPos, 1);
                    if (chr == Environment.NewLine)
                    {
                        //goes to next line...
                        rtnStr += ' ' * (20 - i);
                        break;
                    }
                    if (chr == "\t")
                    {
                        continue; //ignore tab
                    }
                    rtnStr += chr;
                }
            }

            return rtnStr;
        }

        private string FormatDisplayString(string input, string lineReturn)
        {
            string rtnStr = string.Empty;

            for (int row = 0; (row * 20) < input.Length; row++)
            {
                //add a blank row every 4 lines
                if (row % 4 == 0 && row > 0)
                {
                    rtnStr += lineReturn;
                }

                int endChr = 20;
                if (input.Length < ((row * 20) + 20))
                {
                    endChr = input.Length - (row * 20);
                }
                rtnStr += input.Substring((row * 20), endChr);
                rtnStr += lineReturn;
            }

            return rtnStr;
        }

        private void LogMessage(string inputMessage)
        {
            DAL.LCDMessageDBEntities ent = new DAL.LCDMessageDBEntities();
            DAL.MessageLog mess = DAL.MessageLog.CreateMessageLog(0, inputMessage, lblUser.Text, DateTime.Now);
            ent.AddToMessageLogs(mess);
            ent.SaveChanges();
        }
    }

}
