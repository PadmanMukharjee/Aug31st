using M3Pact.LoggerUtility.Helpers;
using M3Pact.LoggerUtility.LogTargets.Implementation;
using M3Pact.LoggerUtility.LogTargets.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace M3Pact.LoggerUtility.Settings
{
    public class DatabaseLoggerSettings : LoggerSettings
    {
        public DatabaseLoggerSettings()
        {
            DbSettings = new DbSettings();
        }
        public DbSettings DbSettings { get; set; }

        internal override List<ILogTarget> GetLogTargets()
        {
            List<ILogTarget> targets = new List<ILogTarget>();
            targets.Add(new DataBase(new DatabaseHelper(this)));
            return targets;
        }
    }

    public class DbSettings
    {
        public DbSettings()
        {
            Columns = new List<Column>();
            AdditionalColumns = new List<Column>();
        }
        public string TableName { get; set; }
        public string ConnectionString { get; set; }
        public List<Column> Columns { get; set; }
        public List<Column> AdditionalColumns { get; set; }
    }
    public class Column
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string MapTo { get; set; }

    }
}
