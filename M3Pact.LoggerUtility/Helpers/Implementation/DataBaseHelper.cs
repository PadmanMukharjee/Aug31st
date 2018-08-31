using M3Pact.LoggerUtility.Helpers.Implementation;
using M3Pact.LoggerUtility.Settings;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace M3Pact.LoggerUtility.Helpers
{
    public class DatabaseHelper : DataBaseHelperBase, IDataBaseHelper
    {
        private readonly DbSettings dbSettings;

        /// <summary>
        /// Initializes the instance variables
        /// </summary>
        /// <param name="level">Log level of the log to be recored</param>
        public DatabaseHelper(DatabaseLoggerSettings settings) : base(settings.LogLevel)
        {
            this.dbSettings = settings.DbSettings;
        }
        /// <summary>
        /// Calls function to log just message along with Date time and log level in the table
        /// </summary>
        /// <param name="message">User defined Message for a log</param>
        public void LogInDB(string message)
        {
            LogInDB(message, null, null);
        }

        /// <summary>
        /// Calls function to log exception details and message along with Date time and log level in the table
        /// </summary>
        /// <param name="message">User defined Message for a log</param>
        /// <param name="e">Exception object whose details need to be logged</param>
        public void LogInDB(string message, Exception e)
        {
            LogInDB(message, e, null);
        }

        /// <summary>
        /// Calls function to log additional details and message along with Date time and log level in the table
        /// </summary>
        /// <param name="message">User defined Message for a log</param>
        /// <param name="logObject">Anonymous object with additional details which need to be logged</param>
        public void LogInDB(string message, dynamic logObject)
        {
            LogInDB(message, null, logObject);
        }

        /// <summary>
        /// logs message, exception and additional details passed in logObject in the Database Table
        /// </summary>
        /// <param name="message">User defined Message for a log</param>
        /// <param name="e">Exception object whose details need to be logged</param>
        /// <param name="logObject">Anonymous object with additional details which need to be logged</param>
        public void LogInDB(string message, Exception exception, dynamic logObject)
        {
            using (var connection = new SqlConnection(dbSettings.ConnectionString))
            {
                connection.Open();
                GenerateLogTableIfRequired(CreateTableQuery(), connection);

                var columns = dbSettings.Columns.Select(x => x.Name).ToList();
                var additionalColumns = dbSettings.AdditionalColumns.Select(x => x.Name).ToList();
                var formatedColumns = GetFormatedColumnNames(columns);
                var formatedAdditionalColumns = GetFormatedColumnNames(additionalColumns);
                formatedColumns.AddRange(formatedAdditionalColumns);
                using (SqlCommand command = new SqlCommand(InsertDataIntoLogTable(formatedColumns, dbSettings.TableName), connection))
                {
                    AddColumnParametersFromSettings(message, exception, command);
                    AddAdditionalColumnParameters(logObject, additionalColumns, command);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Logs additional details specified in the passed anonymous object on to the corresponding columns in the database table
        /// </summary>
        /// <param name="logObject">Anonymous object with additional details which need to be logged</param>
        /// <param name="additionalColumns">List of column names in the database table in which additional details need to be logged</param>
        /// <param name="command">SQL Command object for executing sql command</param>
        private void AddAdditionalColumnParameters(dynamic logObject, List<string> additionalColumns, SqlCommand command)
        {
            foreach (string columnName in additionalColumns)
            {
                PropertyInfo info = logObject?.GetType().GetProperty(columnName);
                string columnValue = Convert.ToString(info?.GetValue(logObject, null));
                CheckNullAndParameterAccordinglyInTable(logObject, columnName, columnValue, command);
            }
        }

        /// <summary>
        /// Logs message and exception details on to the corresponding columns in the database table using logger settings
        /// </summary>
        /// <param name="message">User defined Message for a log</param>
        /// <param name="exception">Exception object whose details need to be logged</param>
        /// <param name="command">SQL Command object for executing sql command</param>
        private void AddColumnParametersFromSettings(string message, Exception exception, SqlCommand command)
        {
            dbSettings.Columns.ForEach(column =>
            {
                AddParameterForMappedColumn(message, exception, command, column.MapTo.ToLower(), column.Name);
            });
        }

        private string CreateTableQuery()
        {
            string partOfTableCreationQuery = string.Join(",", GenerateSubPartOfTableCreationQueryFromSettings());
            string tableCreationQuery = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'" + dbSettings.TableName + "') BEGIN CREATE TABLE " + dbSettings.TableName + " ( Id INT IDENTITY (1,1), " + partOfTableCreationQuery + ")END";
            return tableCreationQuery;
        }

        /// <summary>
        /// Generates a subpart for the table creation query
        /// </summary>
        /// <returns>Subpart of table creation query</returns>
        private string GenerateSubPartOfTableCreationQueryFromSettings()
        {
            List<string> columnsWithType = new List<string>();
            foreach (var column in dbSettings.Columns)
            {
                columnsWithType.Add(column.Name + " " + column.Type);
            }
            return string.Join(",", columnsWithType);
        }
    }
}
