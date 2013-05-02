﻿namespace HSA.InfoSys.WebCrawler
{
    
    using System;
    using System.Net;
    using System.Threading;
    using HSA.InfoSys.Logging;
    using HSA.InfoSys.DBManager;
    using System.Linq;
    using System.Text;
    using log4net;
    using System.IO;
    using System.ServiceModel;

    /// <summary>
    /// The WebCrawler searches the internet
    /// for security issues of several hardware
    /// </summary>
    public class WebCrawler
    {
        private static readonly ILog log = Logging.GetLogger("WebCrawler");

        private WebClient client = new WebClient();

        static void Main(string[] args)
        {
            WebCrawler crawler = new WebCrawler();
            crawler.RunServer();
        }

        private void RunServer()
        {
            using (ServiceHost host = new ServiceHost(typeof(CrawlControler)))
            {
                bool running = true;

                log.Debug("Starting server...");
                log.Info("Press q for quit.");

                host.Open();

                DBManager dbm = new DBManager();

                while (running)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        log.InfoFormat("Got user input key {0}", keyInfo.Key);

                        if (keyInfo.Key == ConsoleKey.Q)
                        {
                            log.Info("User exited the application.");
                            running = false;
                            host.Close();
                        }
                        else
                        {
                            log.Info("Unkown user input.");
                        }
                    }

                    Thread.Sleep(100);
                }
            }
        }

        public string StartSearch()
        {
            log.Info("Search started from GUI");
            return "Hello";
        }
    }
}
