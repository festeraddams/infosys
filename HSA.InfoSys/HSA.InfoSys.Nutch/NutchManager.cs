﻿// ------------------------------------------------------------------------
// <copyright file="NutchManager.cs" company="HSA.InfoSys">
//     Copyright statement. All right reserved
// </copyright>
// ------------------------------------------------------------------------

namespace HSA.InfoSys.Common.Nutch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Diagnostics;


    public class NutchManager
    {

        private string PrefixPath;

        private string URLPath;

        private string FileName = "seed.txt";

        /// <summary>
        /// Initializes a new instance of the <see cref="NutchManager"/> class.
        /// </summary>
        public NutchManager()
        {
            this.URLPath = "C:/Users/A/Desktop/urls";
            this.PrefixPath = "C:/Users/A/Desktop/conf/regex-urlfilter.txt";
        }


        public void MkUserDir(string user)
        {
            string NewDirectory = string.Format("{0}/{1}", this.URLPath, user);
            Directory.CreateDirectory(NewDirectory);
            StreamWriter myWriter = File.CreateText(string.Format("{0}/{1}",NewDirectory, this.FileName));
            myWriter.Close();
        }
  
        public List<string> GetFileContent(string pattern, string filePath)
        {
            List<string> content = new List<string>();

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains(pattern))
                    {
                        content.Add(line);
                    }
                }

              
            }
            return content;
        }


        public void AddURL(List<string> urls, string user)
        {
            string userURLPath = string.Format("{0}/{1}/{2}", this.URLPath, user, this.FileName);

            List<string> prefixUrls = new List<string>();

            foreach (string url in urls)
            {
                string prefix = string.Format("{0}{1}", Properties.Settings.Default.PREFIX, url);

                if(!GetFileContent(Properties.Settings.Default.PREFIX, this.PrefixPath).Contains(prefix))
                {
                    prefixUrls.Add(prefix);
                }
            }
            AddURLToFile(prefixUrls, this.PrefixPath);
            AddURLToFile(urls, userURLPath);
        }




 
        private void AddURLToFile(List<string> urls, string path)
        {
            foreach (string url in urls)
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(url);
                }
            }
        }

        public void startCrawl(string argument)
        {
            ProcessStartInfo process = new ProcessStartInfo();
            process.FileName= "nutch";
            process.Arguments = argument;
            Process.Start(process);
        }

    }
}
