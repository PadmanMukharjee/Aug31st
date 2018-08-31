using M3Pact.Infrastructure.Interfaces.Business.ToDo;
using M3Pact.ViewModel.Todo;
using M3Pact.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace M3Pact.WebAPI.Controllers
{
    [AuthorizationFilter]
    [Route("api/[controller]/[Action]")]
    public class ToDoController : Controller
    {
        private IToDoBusiness _toDoBusiness;
        public ToDoController(IToDoBusiness toDoBusiness)
        {
            _toDoBusiness = toDoBusiness;
        }

        /// <summary>
        /// To get all tasks and respected tasks clients for a user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<TodoViewModel> GetClientsForAllToDoTasks()
        {
            return _toDoBusiness.GetClientsForAllToDoTasks();
        }
    }
}