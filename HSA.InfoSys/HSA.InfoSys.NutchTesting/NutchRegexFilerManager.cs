﻿
namespace HSA.InfoSys.Testing.NutchTesting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using HSA.InfoSys.Common.Logging;
    using log4net;

    class NutchRegexFilerManager
    {

        ILog log = Logging.GetLogger("NutchTesting");

  
        /// <summary>
        /// The filter file
        /// </summary>
        private const string FilterFile = "regex-urlfilter.txt";

        /// <summary>
        /// The path to your nutch - regex - urlfilter file 
        /// </summary>
        private string Path; 

        /// <summary>
        /// The URL prefix
        /// </summary>
        private const string UrlPrefix = "+^http://([a-z0-9]*\\.)*";


        /// <summary>
        /// Array contains all flters
        /// </summary>
        private List<string> RegexFilters;

        /// <summary>
        /// Initializes a new instance of the <see cref="NutchRegexFilerManager"/> class and 
        /// puts found filters into RegexFilters array.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException"></exception>
        public NutchRegexFilerManager(string YourPath)
        {
            Path = string.Format("{0}{1}", YourPath, FilterFile);

            if (!File.Exists(Path))
            {
                log.Error(string.Format("Your Path is not correctly set! Actual Path: {0}", Path));
                throw new FileNotFoundException();
            }

            RegexFilters = new List<string>();
            using (StreamReader sr = new StreamReader(Path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains(UrlPrefix))
                    {
                        RegexFilters.Add(line);
                    }
                }
            } 
        }
        

        /// <summary>
        /// Adds the URL to the filter file.
        /// </summary>
        /// <param name="NewUrl">The new URL.</param>
        public void AddUrl(string NewUrl)
        {   
            string FilterRule = string.Format("{0}{1}", UrlPrefix, NewUrl);
            if (!RegexFilters.Contains(FilterRule))
            {
                using (StreamWriter sw = File.AppendText(Path))
                {
                    sw.WriteLine(FilterRule);
                }

                RegexFilters.Add(FilterRule);
                log.Info(string.Format("New FilterRule Added: {0}", FilterRule));
            }
        }


        /// <summary>
        /// Gets the regex filters.
        /// </summary>
        /// <returns></returns>
        public List<string> GetRegexFilters()
        {
            return RegexFilters;
        }

    }
}