using M3Pact.BusinessModel.BusinessModels;
using M3Pact.BusinessModel.CheckList;
using M3Pact.Infrastructure.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace M3Pact.Infrastructure.Interfaces.Repository.Admin
{
    public interface ICheckListRepository
    {
        Task<List<Question>> GetQuestionsOfChecklistType(string checklistType);
        Task<Question> CreateQuestion(Question question);
        Task<Question> UpdateQuestion(Question question);
        List<ViewChecklist> GetAllChecklists();
        Task<CheckList> CreateCheckListAsync(CheckList question);
        Task<List<BusinessModel.BusinessModels.CheckListType>> GetCheckListTypesAsync();
        Task<BusinessModel.BusinessModels.CheckList> GetCheckListByIdAsync(int id);
        bool CheckClientDependencyOnDelteSystem(int systemId, int checkListId);
        bool CheckClientDependencyOnDelteSite(int siteId, int checkListId);
        Task<BusinessModel.BusinessModels.CheckList> UpdateCheckListAsync(BusinessModel.BusinessModels.CheckList checkList);
        Task<List<BusinessModel.BusinessModels.CheckList>> GetCheckListByQuery(CheckListQueryModel checkListQueryModel);
        List<string> GetChecklistHeatMapQuestions();
    }
}
