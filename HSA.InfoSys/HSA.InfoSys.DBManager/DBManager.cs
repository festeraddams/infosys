﻿namespace HSA.InfoSys.DBManager
{
    using System;
    using System.Linq;
    using HSA.InfoSys.DBManager.Data;
    using HSA.InfoSys.Logging;
    using log4net;

    /// <summary>
    /// The DBManager handles database requests.
    /// </summary>

    public class DBManager : IDBManager
    {
        private static readonly ILog log = Logging.GetLogger("DBManager");

        /// <summary>
        /// The database manager.
        /// </summary>
        private static IDBManager dbManager;

        /// <summary>
        /// Constructor. creates a configuration Object and add the DBManger as
        /// Assembly
        /// has also the DB-Schema of Nhibernate
        /// </summary>
        private DBManager()
        {
        }

        /// <summary>
        /// Gets the DB manager and asures that the configuration
        /// will be executed only once and that there is only one dbmanager.
        /// </summary>
        /// <returns>The Database Manager</returns>
        public static IDBManager GetDBManager()
        {
            if (dbManager == null)
            {
                log.Debug("DBManager does not exist, create one...");
                dbManager = new DBManager();
            }
            
            return dbManager;
        }

        /// <summary>
        /// adds a new Object (Component, Issue, Source)
        /// and saves it in database
        /// </summary>
        /// <param name="obj">Object</param>
        public void AddNewObject(object obj)
        {
            DBSession.Add(obj);
            log.Info("Instance saved successfully in database");
        }

        /// <summary>
        /// saves changings of a object in database
        /// </summary>
        /// <param name="obj">Object</param>
        public void UpdateObject(object obj)
        {
            

            log.Info("Instance updated successfully in database");
        }

        /// <summary>
        /// returns a Component-Object 
        /// </summary>
        /// <param name="componentGUID">Id of the Object</param>
        /// <returns>Component-Object</returns>
        public Component GetComponent(Guid componentGUID)
        {
            Component component = DBSession.Query<Component>("SELECT * FROM Component")
                .Where(x => x.componentGUID == componentGUID)
                .SingleOrDefault();
            
            log.InfoFormat("Got component {0} with GUID {1}", component, componentGUID);

            return component;
        }

        /// <summary>
        /// return a Issue-Object from database
        /// </summary>
        /// <param name="issueGUID">Id of the Object </param>
        /// <returns>Issue-Object</returns>
        public Issue GetIssue(Guid issueGUID)
        {
            Issue issue = DBSession.Query<Issue>("SELECT * FROM Issue")
                .Where(x => x.issueGUID == issueGUID)
                .SingleOrDefault();

            log.InfoFormat("Got issue {0} with GUID {1}", issue, issueGUID);

            return issue;
        }

        /// <summary>
        /// returns a Source-Object from database
        /// </summary>
        /// <param name="sourceGUID">Id of the Object</param>
        /// <returns>Source-Object</returns>
        public Source GetSource(Guid sourceGUID)
        {
            Source source = DBSession.Query<Source>("SELECT * FROM Source")
                .Where(x => x.sourceGUID == sourceGUID)
                .SingleOrDefault();

            log.InfoFormat("Got source {0} with GUID {1}", source, sourceGUID);

            return source;  
        }

        /// <summary>
        /// creates an Component-Object and returns it
        /// </summary>
        /// <param name="componentName">name of the component</param>
        /// <param name="componentCategory">categroy of the component</param>
        /// <returns>Component-Object</returns>
        public Component CreateComponent(string componentName, string componentCategory)
        {
            var component = new Component
            { 
                componentGUID = System.Guid.NewGuid(),
                category = componentCategory,
                name = componentName
            };

            log.InfoFormat("New component [{0}] created.", component);

            return component;
        }

        /// <summary>
        /// creates an Source-Object and returns it
        /// </summary>
        /// <param name="sourceURL">URL</param>
        /// <returns> Source-Object</returns>
        public Source CreateSource(string sourceURL)
        {
            var source = new Source
            { 
                sourceGUID = System.Guid.NewGuid(),
                URL = sourceURL
            };

            log.InfoFormat("New source [{0}] created.", source);

            return source;
        }
    }
}