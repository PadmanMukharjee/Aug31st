using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace M3Pact.LoggerUtility.Helpers.Implementation
{
    public class DataBaseHelperBase
    {
        private readonly LogLevel _logLevel;
        private static bool isTableCreated;

        public DataBaseHelperBase(LogLevel logLevel)
        {
            _logLevel = logLevel;
        }
        /// <summary>
        /// Creates query for insertion in the database table
        /// </summary>
        /// <param name="columns">List of columns in the table for which data need to be recorded</param>
        /// <returns>Table insertion Query</returns>
        internal string InsertDataIntoLogTable(List<string> columns, string tableName)
        {
            string partOfInsertionQuery = string.Join(",", columns);
            string insertionQuery = "INSERT INTO " + tableName + "(" + partOfInsertionQuery.Replace("@", "") + ") VALUES(" + partOfInsertionQuery + ")";
            return insertionQuery;
        }

        /// <summary>
        /// Adds parameter to the corresponding mapped column in the database table
        /// </summary>
        /// <param name="message">User defined Message for a log</param>
        /// <param name="e">Exception object whose details need to be logged</param>
        /// <param name="command">SQL Command object for executing sql command</param>
        /// <param name="mappedTo">Attribute in DBConfig to denote which column is mappedTo which parameter</param>
        /// <param name="columnName">Column Name of Column in the Database table for which value need to be added</param>
        internal void AddParameterForMappedColumn(string message, Exception e, SqlCommand command, string mappedTo, string columnName)
        {
            switch (mappedTo)
            {
                case Constants.Time:
                    command.Parameters.Add(new SqlParameter(columnName, System.DateTime.Now));
                    break;

                case Constants.LogLevel:
                    command.Parameters.Add(new SqlParameter(columnName, _logLevel.ToString()));
                    break;

                case Constants.Message:
                    command.Parameters.Add(new SqlParameter(columnName, message));
                    break;

                case Constants.Exception:
                    CheckNullAndParameterAccordinglyInTable(e, columnName, e?.Message, command);
                    break;

                case Constants.StackTrace:
                    CheckNullAndParameterAccordinglyInTable(e, columnName, e?.StackTrace, command);
                    break;

                default:
                    throw new Exception(Constants.ExceptionMessage_MappedToNotFound);
            }
        }

        /// <summary>
        /// Adds null value in the database table column if the object do not exists
        /// </summary>
        /// <param name="obj">Any object whose value need to be checked against null</param>
        /// <param name="columnName">Column Name of the table in which data need to be added</param>
        /// <param name="columnValue">Value which need to be added in the Database Table Column</param>
        /// <param name="command">SQL Command object for executing sql command</param>
        internal void CheckNullAndParameterAccordinglyInTable(object obj, string columnName, String columnValue, SqlCommand command)
        {
            if (obj == null)
            {
                command.Parameters.Add(new SqlParameter(columnName, DBNull.Value));
            }
            else
            {
                command.Parameters.Add(new SqlParameter(columnName, columnValue));
            }
        }

        /// <summary>
        /// Checks if the Table Required exists.If not it creates the table and set flag to true to denote that table exists now
        /// </summary>
        /// <param name="connection">SQL Connection object to make connection with the database</param>
        internal void GenerateLogTableIfRequired(string Query, SqlConnection connection)
        {
            if (!isTableCreated)
            {
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.ExecuteNonQuery();
                    isTableCreated = true;
                }
            }
        }

        /// <summary>
        /// Provides list of column names in the table with '@' prepended to the column names
        /// </summary>
        /// <param name="columnNames">List of column names in the table for which data need to be inserted</param>
        /// <returns>List of column names with '@' prepended to the column names</returns>
        internal List<string> GetFormatedColumnNames(IEnumerable<string> columnNames)
        {
            var formatedColumnNames = new List<string>();
            foreach (var column in columnNames)
            {
                formatedColumnNames.Add(string.Concat("@", column));
            }
            return formatedColumnNames;
        }
    }
}
