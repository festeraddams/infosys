﻿// ------------------------------------------------------------------------
// <copyright file="SolrJsonParser.cs" company="HSA.InfoSys">
//     Copyright statement. All right reserved
// </copyright>
//------------------------------------------------------------------------- 
namespace HSA.InfoSys.Common.SolrClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using HSA.InfoSys.Common.Logging;
    using log4net;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using HSA.InfoSys.Common.Entities;

    /// <summary>
    /// SolrJsonParser parses the response of SolrClient
    /// </summary>
    public class SolrJsonParser
    {
        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog Log = Logger<string>.GetLogger("SolrJsonParser");

        /// <summary>
        /// The results list in which all Results can be found
        /// </summary>
        private List<Result> results = new List<Result>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SolrJsonParser"/> class.
        /// </summary>
        public SolrJsonParser()
        {
            this.results = new List<Result>();
        }

        /// <summary>
        /// Parses to string.
        /// </summary>
        /// <param name="result">The result.</param>
        public void ParseToString(string result)
        {
            var json = JsonConvert.DeserializeObject(result) as JObject;
            var response = json["response"];
            var docs = response["docs"];

            foreach (var doc in docs)
            {
                var r = new Result();

                r.Content = doc["content"].ToString();
                r.URL = doc["url"].ToString();
                r.Title = doc["title"].ToString();
                r.Time = (DateTime)doc["tstamp"];

                this.results.Add(r);

                Log.Info(string.Format("Search resualt was added! ", r.Title));
            }
        }

        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <returns>The results</returns>
        public List<Result> GetResults()
        {
            return this.results;
        }
    }
}
