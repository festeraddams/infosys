// ------------------------------------------------------------------------
// <copyright file="NutchTesting.cs" company="HSA.InfoSys">
//     Copyright statement. All right reserved
// </copyright>
// ------------------------------------------------------------------------
namespace HSA.InfoSys.Testing.NutchTesting
{
    using System;
    using System.Threading;
    using HSA.InfoSys.Common.Logging;
    using HSA.InfoSys.Common.Services.LocalServices;
    using log4net;
    using HSA.InfoSys.Common.Entities;

    /// <summary>
    /// Implement your testing methods for Nutch here.
    /// </summary>
    public class NutchTesting
    {
        /// <summary>
        /// The logger for NutchTesting.
        /// </summary>
        private static readonly ILog Log = Logger<string>.GetLogger("NutchTesting");

        /// <summary>
        /// Mains the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public static void Main(string[] args)
        {
            //// var controller = NutchController.NutchFactory(Guid.NewGuid(), "http://miitsoft.de");
            bool running = true;
           
            Console.WriteLine(string.Empty);
            Console.WriteLine("Here you can test the nutch funcionality.");
            Console.WriteLine("To see your options press h or press q for quit.");
            Console.WriteLine(string.Empty);

            while (running)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    Log.InfoFormat("Key [{0}] was pressed.", keyInfo.Key);

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.A:
                            Log.Info("Add new job and start crawling");
                            //// controller.SetNextCrawl();
                            break;
                        case ConsoleKey.C:
                            Log.Info("Connect to SSH.");
                            break;

                        case ConsoleKey.H:
                            Log.Info("Print help text.");
                            PrintHelp();
                            break;

                        case ConsoleKey.P:
                            break;

                        case ConsoleKey.Q:
                            Log.Info("Quit application.");
                            running = false;
                            break;

                        case ConsoleKey.S:
                            Log.Info("Send request to nutch.");
                            NutchControllerClientSettings ns1 = new NutchControllerClientSettings();
                            NutchControllerClientSettings ns2 = new NutchControllerClientSettings();
                            ns2.CrawlTopN = 9;
                            WCFSettings ws = new WCFSettings();

                            if (ns1.Equals(ns2))
                            {
                                Log.Info("Equal");
                            }
                            else
                            {
                                Log.Info("Not equal");
                            }

                            break;
                    }
                }

                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Prints the help.
        /// </summary>
        private static void PrintHelp()
        {   
            Console.WriteLine(string.Empty);
            Console.WriteLine("Press h to see this help text.");
            Console.WriteLine("Press q to quit this application.");
            Console.WriteLine("Press s to start a new request to nutch server.");
            Console.WriteLine(string.Empty);
        }
    }
}
