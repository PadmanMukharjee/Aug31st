using M3Pact.ViewModel.Todo;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Business.ToDo
{
    public interface IToDoBusiness
    {
        List<TodoViewModel> GetClientsForAllToDoTasks();
    }
}
