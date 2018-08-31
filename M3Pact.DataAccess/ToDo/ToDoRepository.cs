using M3Pact.BusinessModel.Todo;
using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Repository.ToDo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace M3Pact.Repository.ToDo
{
    public class ToDoRepository : IToDoRepository
    {
        #region properties
        private M3PactContext _m3PactContext;
        private IConfiguration _configuration { get; }
        private UserContext userContext;
        #endregion properties

        #region Constructor
        public ToDoRepository(M3PactContext m3PactContext, IConfiguration configuration)
        {
            _m3PactContext = m3PactContext;
            _configuration = configuration;
            userContext = UserHelper.getUserContext();
        }
        #endregion

        #region public methods      
        /// <summary>
        /// To get all tasks with clients
        /// </summary>
        /// <returns></returns>
        public List<TodoBusinessModel> GetClientsForAllToDoTasks()
        {
            try
            {
                List<TodoBusinessModel> todoBusinessModels = new List<TodoBusinessModel>();
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_configuration, "M3PactConnection");
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    string procedure = DomainConstants.GetToDoListActions;
                    using (SqlCommand sqlCmd = new SqlCommand(procedure, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@role", userContext.Role);
                        sqlCmd.Parameters.AddWithValue("@UserId", userContext.UserId);
                        sqlConn.Open();
                        SqlDataReader reader = sqlCmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    TodoBusinessModel todoBusinessModel = new TodoBusinessModel();
                                    todoBusinessModel.ClientId = reader["ClientId"] != DBNull.Value ? Convert.ToInt32(reader["ClientId"]) : 0;
                                    todoBusinessModel.ClientCode = reader["ClientCode"] != DBNull.Value ? reader["ClientCode"].ToString() : "";
                                    todoBusinessModel.ClientName = reader["ClientName"] != DBNull.Value ? reader["ClientName"]?.ToString() : "";
                                    todoBusinessModel.TaskName = reader["ActionName"] != DBNull.Value ? reader["ActionName"]?.ToString() : "";
                                    todoBusinessModels.Add(todoBusinessModel);
                                }
                                reader.NextResult();
                            }
                        }
                    }
                }
                return todoBusinessModels;
            }
            catch
            {
                throw;
            }
        }
        #endregion        
    }
}
