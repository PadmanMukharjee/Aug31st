using M3Pact.BusinessModel.Todo;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Repository.ToDo
{
    public interface IToDoRepository
    {
        List<TodoBusinessModel> GetClientsForAllToDoTasks();
    }
}
