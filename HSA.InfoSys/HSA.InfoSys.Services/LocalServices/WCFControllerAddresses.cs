﻿// ------------------------------------------------------------------------
// <copyright file="WCFControllerAddresses.cs" company="HSA.InfoSys">
//     Copyright statement. All right reserved
// </copyright>
// ------------------------------------------------------------------------
namespace HSA.InfoSys.Common.Services.LocalServices
{
    using System;
    using System.Collections.Generic;
    using HSA.InfoSys.Common.Entities;
    using HSA.InfoSys.Common.Services.WCFServices;

    /// <summary>
    /// This is a helper class to get the correct service address by its type.
    /// </summary>
    public static class WCFControllerAddresses
    {
        /// <summary>
        /// The HTTP format.
        /// </summary>
        private const string HttpFormat = "http://{0}:{1}/{2}/";

        /// <summary>
        /// The net TCP format.
        /// </summary>
        private const string NetTcpFormat = "net.tcp://{0}:{1}/{2}/";

        /// <summary>
        /// The net TCP addresses.
        /// </summary>
        private static readonly Dictionary<Type, string> NetTcpAddresses = new Dictionary<Type, string>();

        /// <summary>
        /// The net TCP addresses.
        /// </summary>
        private static readonly Dictionary<Type, string> HttpAddresses = new Dictionary<Type, string>();

        /// <summary>
        /// The HTTP port.
        /// </summary>
        private static int httpPort;

        /// <summary>
        /// The net TCP port.
        /// </summary>
        private static int netTcpPort;

        /// <summary>
        /// Indicates if addresses are initialized.
        /// </summary>
        private static bool notInitialized = true;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public static void Initialize(WCFSettings settings)
        {
            if (notInitialized)
            {
                var types = new[]
                {
                    typeof(ICrawlController),
                    typeof(ICrawlerService),
                    typeof(ISolrController),
                    typeof(IScheduler),
                    typeof(IDbManager)
                };

                httpPort = settings.HttpPort;
                netTcpPort = settings.NetTcpPort;

                foreach (var t in types)
                {
                    var httpAddress = string.Format(
                        HttpFormat,
                        settings.HttpHost,
                        httpPort,
                        settings.HttpPath);

                    var netTcpAddress = string.Format(
                        NetTcpFormat,
                        settings.NetTcpHost,
                        netTcpPort,
                        settings.NetTcpPath);

                    HttpAddresses.Add(t, httpAddress);
                    NetTcpAddresses.Add(t, netTcpAddress);

                    httpPort += 2;
                    netTcpPort += 2;
                }

                notInitialized = false;
            }
        }

        /// <summary>
        /// Gets the net TCP address.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Returns the net TCP address by type.</returns>
        public static string GetNetTcpAddress(Type type)
        {
            return NetTcpAddresses.ContainsKey(type) ? NetTcpAddresses[type] : string.Empty;
        }

        /// <summary>
        /// Gets the HTTP address.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Returns the http address by type.</returns>
        public static string GetHttpAddress(Type type)
        {
            return HttpAddresses.ContainsKey(type) ? HttpAddresses[type] : string.Empty;
        }
    }
}
