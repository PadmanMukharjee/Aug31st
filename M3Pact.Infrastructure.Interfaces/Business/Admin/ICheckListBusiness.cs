using M3Pact.BusinessModel.BusinessModels;
using M3Pact.Infrastructure.Common;
using M3Pact.ViewModel.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace M3Pact.Infrastructure.Interfaces.Business.Admin
{
    public interface ICheckListBusiness
    {
        Task<List<Question>> GetQuestions(string checklistType);
        Task<Question> CreateQuestion(CheckListItemViewModel question);
        Task<Question> UpdateQuestion(CheckListItemViewModel question);
        List<AllChecklistViewModel> GetAllChecklists();
        Task<CheckList> CreateCheckList(CheckList checkList);
        Task<List<CheckListType>> GetCheckListTypes();
        Task<CheckList> GetCheckListById(int id);
        bool CheckClientDependencyOnDelteSystem(int systemId, int checkListId);
        bool CheckClientDependencyOnDelteSite(int siteId, int checkListId);
        Task<CheckList> UpdateCheckList(CheckList checkList);
        Task<List<CheckList>> GetCheckListByQuery(CheckListQueryModel checkListQueryModel);
        List<string> GetChecklistHeatMapQuestions();
        //TODO:
        //Task<List<CheckList>> GetCheckLists();
    }
}
