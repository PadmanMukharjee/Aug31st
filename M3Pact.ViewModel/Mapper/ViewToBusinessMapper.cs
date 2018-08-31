using M3Pact.BusinessModel.BusinessModels;
using System;

namespace M3Pact.ViewModel.Mapper
{
    public static class ViewToBusinessMapper
    {
        public static CheckList CheckListMap(this M3Pact.ViewModel.Admin.CheckListViewModel checkListViewModel)
        {
            CheckList checkList;
            if (checkListViewModel == default(M3Pact.ViewModel.Admin.CheckListViewModel))
            {
                return new CheckList();
            }

            try
            {
                checkList = new CheckList
                {
                    CheckListId = checkListViewModel.ChecklistId,
                    CheckListName = checkListViewModel.Name,
                    CheckListType = checkListViewModel.ChecklistType,
                    Questions = checkListViewModel.ChecklistItems,
                    Sites = checkListViewModel.Sites,
                    Systems = checkListViewModel.Systems                    
                };

                return checkList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
