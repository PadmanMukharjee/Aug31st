using M3Pact.BusinessModel.Todo;
using M3Pact.Infrastructure.Interfaces.Business.ToDo;
using M3Pact.Infrastructure.Interfaces.Repository.ToDo;
using M3Pact.LoggerUtility;
using M3Pact.ViewModel.Todo;
using System;
using System.Collections.Generic;

namespace M3Pact.Business.ToDo
{
    public class ToDoBusiness : IToDoBusiness
    {
        private IToDoRepository _toDoRepository;
        private ILogger _logger;

        #region constructor
        public ToDoBusiness(IToDoRepository toDoRepository,ILogger logger)
        {
            _toDoRepository = toDoRepository;
            _logger = logger;
        }
        #endregion constructor

        #region public methods
        /// <summary>
        /// to get tasks for a user and clients for tasks
        /// </summary>
        /// <returns></returns>
        public List<TodoViewModel> GetClientsForAllToDoTasks()
        {
            try
            {
                List<TodoBusinessModel> todoBusinessModels = _toDoRepository.GetClientsForAllToDoTasks();
                return BusinessMapper.MappingTodoBusinessToViewModel(todoBusinessModels);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }
        #endregion public methods       
    }
}
