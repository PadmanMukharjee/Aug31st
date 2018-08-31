using M3Pact.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace M3Pact.ViewModel.Mapper
{
    public static class BusinessToViewMapper
    {
        /// <summary>
        /// mapper utility to convert a business checklist type list to a KeyValueModel list
        /// </summary>
        /// <param name="checkListType"></param>
        /// <returns></returns>
        public static List<KeyValueModel> CheckListTypeMap(this IEnumerable<M3Pact.BusinessModel.BusinessModels.CheckListType> checkListType)
        {
            if (checkListType == default(List<M3Pact.BusinessModel.BusinessModels.CheckListType>))
            {
                return new List<KeyValueModel>();
            }

            try
            {
                return checkListType.Select(e =>
                {
                    KeyValueModel j = new KeyValueModel
                    {
                        Key = e.CheckListTypeID.ToString(),
                        Value = e.CheckListTypeName
                    };
                    return j;
                }).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// mapper utility to convert a business checklist to a checklist view model
        /// </summary>
        /// <param name="checkList"></param>
        /// <returns></returns>
        public static M3Pact.ViewModel.Admin.CheckListViewModel CheckListMap(this M3Pact.BusinessModel.BusinessModels.CheckList checkList)
        {
            M3Pact.ViewModel.Admin.CheckListViewModel checkListViewModel;
            if (checkList == default(M3Pact.BusinessModel.BusinessModels.CheckList))
            {
                return new M3Pact.ViewModel.Admin.CheckListViewModel();
            }

            try
            {
                checkListViewModel = new M3Pact.ViewModel.Admin.CheckListViewModel
                {
                    ChecklistId = checkList.CheckListId,
                    ChecklistItems = checkList.Questions,
                    ChecklistType = checkList.CheckListType,
                    Name = checkList.CheckListName,
                    Sites = checkList.Sites,
                    Systems = checkList.Systems
                };

                return checkListViewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkList"></param>
        /// <returns></returns>
        public static List<KeyValueModel> KeyValueMap(this List<M3Pact.BusinessModel.BusinessModels.CheckList> checkList)
        {
            List<KeyValueModel> keyValueList;
            if (checkList == default(List<M3Pact.BusinessModel.BusinessModels.CheckList>))
            {
                return new List<KeyValueModel>();
            }

            try
            {
                keyValueList = checkList.Select(b => new KeyValueModel
                {
                    Key = b.CheckListId.ToString(),
                    Value = b.CheckListName
                }).ToList();

                return keyValueList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
