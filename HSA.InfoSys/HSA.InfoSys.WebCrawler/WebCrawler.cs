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
    using System.ServiceModel.Channels;
    using NHibernate;
    using HSA.InfoSys.DBManager.Data;
  
   
    

    /// <summary>
    /// The WebCrawler searches the internet
    /// for security issues of several hardware
    /// </summary>
    public class WebCrawler : ICrawlControler
    {
        private static readonly ILog log = Logging.GetLogger("WebCrawler");

        /// <summary>
        /// The running flag for this server.
        /// </summary>
        private static bool running;

        /// <summary>
        /// The service host for communication between server and gui.
        /// </summary>
        private ServiceHost host;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebCrawler"/> class.
        /// </summary>
        public WebCrawler()
        {
            host = new ServiceHost(typeof(WebCrawler));
        }

        /// <summary>
        /// Main function.
        /// </summary>
        /// <param name="args">The args.</param>
        static void Main(string[] args)
        {
            WebCrawler crawler = new WebCrawler();
            crawler.RunServer();
        }

        /// <summary>
        /// Runs the server.
        /// </summary>
        private void RunServer()
        {
            log.Debug("Starting server...");
            log.Info("Press q for quit.");

            host.Open();

            
           // dbm.addNewComponent(System.Guid.NewGuid(), 122, "test1", "test1");


            var comp = new Component { componentGUID = System.Guid.NewGuid(), componentId = 884, category = "test1", name = "test12" };
            var source = new Source { sourceGUID = System.Guid.NewGuid(), sourceId = 123, URL = "www.google.de" };
            
            IDBManager dbm = new DBManager();
            dbm.addNewObject(comp);
            
           
            running = true;

            while (running)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    log.InfoFormat("Got user input key {0}", keyInfo.Key);

                    if (keyInfo.Key == ConsoleKey.Q)
                    {
                        log.Info("User exited the application.");
                        Shutdown();
                    }
                    else
                    {
                        log.Info("Unkown user input.");
                    }
                }

                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Starts a new search.
        /// </summary>
        /// <returns></returns>
        public string StartSearch()
        {
            log.Info("Search started from GUI");
            return "Hello";
        }

        /// <summary>
        /// Shuts down web crawler.
        /// </summary>
        /// <returns>true on success.</returns>
        public bool ShutDownWebCrawler()
        {
            Shutdown();
            return true;
        }

        /// <summary>
        /// Shutdown this instance.
        /// </summary>
        private void Shutdown()
        {
            if (host != null)
            {
                running = false;
                host.Close();
            }
        }
    }
}
