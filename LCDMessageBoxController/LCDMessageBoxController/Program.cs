using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace LCDMessageBoxController
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (!Properties.Settings.Default.RunAsService || (args.Length > 0 && args[0] == "nonservice"))
            {
                LCDMessageBoxController controller = new LCDMessageBoxController();
                controller.Start();
                while (true)
                {
                    Thread.Sleep(new TimeSpan(1,0,0));
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
			    { 
				    new LCDMessageBoxController() 
			    };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
