﻿// ------------------------------------------------------------------------
// <copyright file="CrawlerService.cs" company="HSA.InfoSys">
//     Copyright statement. All right reserved
// </copyright>
// ------------------------------------------------------------------------
namespace HSA.InfoSys.CrawlerService
{
    using System;
    using System.Threading;
    using HSA.InfoSys.Common.Logging;
    using HSA.InfoSys.Common.Services;
    using HSA.InfoSys.Common.Services.LocalServices;
    using HSA.InfoSys.Common.Services.WCFServices;
    using log4net;

    /// <summary>
    /// The WebCrawler searches the internet
    /// for security issues of several hardware
    /// </summary>
    public class CrawlerService : Service
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILog Log = Logger<string>.GetLogger("CrawlerService");

        /// <summary>
        /// The running flag for this server.
        /// </summary>
        private bool running;

        /// <summary>
        /// The services running flag.
        /// </summary>
        private bool servicesRunning = false;

        /// <summary>
        /// The WCF controller for the crawler service.
        /// </summary>
        private WCFControllerHost controllerHost;

        /// <summary>
        /// The crawl controller.
        /// </summary>
        private CrawlController crawlController;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrawlerService"/> class.
        /// </summary>
        /// <param name="serviceGUID">The service GUID.</param>
        public CrawlerService(Guid serviceGUID) : base(serviceGUID)
        {
        }

        /// <summary>
        /// Main function.
        /// </summary>
        /// <param name="args">The args.</param>
        public static void Main(string[] args)
        {
            CrawlerService crawler = new CrawlerService(Guid.NewGuid());
            crawler.StartService();
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        protected override void Run()
        {
            Log.Debug(Properties.Resources.WEB_CRAWLER_START_SERVER);
            Log.Info(Properties.Resources.WEB_CRAWLER_QUIT_MESSAGE);

            this.InitializeControllerHost();
            this.servicesRunning = this.crawlController.StartServices();

            this.running = true;

            while (this.running)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    Log.InfoFormat(Properties.Resources.WEB_CRAWLER_GOT_USERINPUT, keyInfo.Key);

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.Q:
                            Log.Info(Properties.Resources.WEB_CRAWLER_EXITED_BY_USER);
                            this.ShutdownCrawler();
                            break;

                        case ConsoleKey.S:
                            if (this.servicesRunning)
                            {
                                this.servicesRunning = this.crawlController.StopServices(true);
                            }
                            else
                            {
                                this.servicesRunning = this.crawlController.StartServices();
                            }

                            break;

                        default:
                            Log.Info(Properties.Resources.WEB_CRAWLER_UNKOWN_USERINPUT);
                            break;
                    }
                }

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Initializes the controller host.
        /// </summary>
        private void InitializeControllerHost()
        {
            WCFControllerAddresses.Initialize();
            this.controllerHost = new WCFControllerHost();

            this.crawlController = this.controllerHost.OpenWCFHost<CrawlController, ICrawlController>(CrawlController.ControllerFactory(Guid.NewGuid()));

            var dbManager = this.controllerHost.OpenWCFHost<DBManager, IDBManager>(DBManager.ManagerFactory(Guid.NewGuid()) as DBManager);
            this.crawlController.RegisterService(dbManager);

            var solrController = this.controllerHost.OpenWCFHost<SolrController, ISolrController>(SolrController.SolrFactory(Guid.NewGuid()));
            this.crawlController.RegisterService(solrController);

            var scheduler = this.controllerHost.OpenWCFHost<Scheduler, IScheduler>(Scheduler.SchedulerFactory(Guid.NewGuid()));
            this.crawlController.RegisterService(scheduler);
        }

        /// <summary>
        /// Shutdown this instance.
        /// </summary>
        private void ShutdownCrawler()
        {
            if (this.controllerHost != null)
            {
                this.crawlController.StopServices(true);
                this.controllerHost.CloseWCFHosts();

                this.running = false;
            }
        }
    }
}
