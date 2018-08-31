using M3Pact.BusinessModel.BusinessModels;
using M3Pact.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace M3Pact.Repository.Mappers
{
    /// <summary>
    /// RepoToBusinessMapper acts as a mapper between domain entities and business entities
    /// </summary>
    public static class RepoToBusinessMapper
    {

        /// <summary>
        /// to map a list of checklisttype domain entity with a list of checklitstype business entity       
        /// </summary>
        /// <param name="checkListType"></param>
        /// <returns></returns>
        public static List<CheckListType> CheckListTypeMap(this IEnumerable<M3Pact.DomainModel.DomainModels.CheckListType> checkListType)
        {
            if (checkListType == default(List<M3Pact.DomainModel.DomainModels.CheckListType>))
            {
                return new List<CheckListType>();
            }
            try
            {
                return checkListType.Select(e =>
                {
                    CheckListType j = new CheckListType
                    {
                        CheckListTypeCode = e.CheckListTypeCode,
                        CheckListTypeID = e.CheckListTypeId,
                        CheckListTypeName = e.CheckListTypeName,
                        RecordStatus = e.RecordStatus
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
        /// To map checklist from repo to business
        /// </summary>
        /// <param name="checkList"></param>
        /// <returns></returns>
        public static CheckList CheckListMap(this M3Pact.DomainModel.DomainModels.CheckList checkList, M3Pact.DomainModel.DomainModels.CheckListAttribute systemAttribute, M3Pact.DomainModel.DomainModels.CheckListAttribute siteAttribute, M3Pact.DomainModel.DomainModels.CheckListAttribute questionAttribute)
        {
            CheckList checklist;
            if (checkList == default(M3Pact.DomainModel.DomainModels.CheckList))
            {
                return new CheckList();
            }
            try
            {
                checklist = new CheckList
                {
                    CheckListId = checkList.CheckListId,
                    CheckListName = checkList.CheckListName,
                    CheckListType = new KeyValueModel
                    {
                        Key = checkList.CheckListTypeId.ToString(),
                        Value = checkList.CheckListType != null ? checkList.CheckListType.CheckListTypeName : string.Empty
                    },
                    Questions = checkList.CheckListAttributeMap
                    .Where(r => r.CheckListAttributeId == questionAttribute.CheckListAttributeId)
                    .Select(g => new KeyValueModel
                    {
                        Key = g.CheckListAttributeValueId,
                        Value = string.Empty
                    }).ToList(),
                    Systems = checkList.CheckListAttributeMap
                    .Where(r => r.CheckListAttributeId == systemAttribute.CheckListAttributeId)
                    .Select(g => new KeyValueModel
                    {
                        Key = g.CheckListAttributeValueId,
                        Value = string.Empty
                    }).ToList(),
                    Sites = checkList.CheckListAttributeMap
                    .Where(r => r.CheckListAttributeId == siteAttribute.CheckListAttributeId)
                    .Select(g => new KeyValueModel
                    {
                        Key = g.CheckListAttributeValueId,
                        Value = string.Empty
                    }).ToList()
                };

                return checklist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To map a list of checklist from repo to business
        /// </summary>
        /// <param name="checkList"></param>
        /// <returns></returns>
        public static List<CheckList> CheckListMap(this IEnumerable<M3Pact.DomainModel.DomainModels.CheckList> checkList, M3Pact.DomainModel.DomainModels.CheckListAttribute systemAttribute, M3Pact.DomainModel.DomainModels.CheckListAttribute siteAttribute, M3Pact.DomainModel.DomainModels.CheckListAttribute questionAttribute)
        {
            List<CheckList> checklist;
            if (checkList == default(List<M3Pact.DomainModel.DomainModels.CheckList>) || !checkList.Any())
            {
                return new List<CheckList>();
            }
            try
            {
                checklist = checkList.Select(rl => new CheckList
                {
                    CheckListId = rl.CheckListId,
                    CheckListName = rl.CheckListName,
                    CheckListType = new KeyValueModel
                    {
                        Key = rl.CheckListTypeId.ToString(),
                        Value = rl.CheckListType != null ? rl.CheckListType.CheckListTypeName : string.Empty
                    },
                    Questions = rl.CheckListAttributeMap
                    .Where(r => r.CheckListAttributeId == questionAttribute.CheckListAttributeId)
                    .Select(g => new KeyValueModel
                    {
                        Key = g.CheckListAttributeValueId,
                        Value = string.Empty
                    }).ToList(),
                    Systems = rl.CheckListAttributeMap
                    .Where(r => r.CheckListAttributeId == systemAttribute.CheckListAttributeId)
                    .Select(g => new KeyValueModel
                    {
                        Key = g.CheckListAttributeValueId,
                        Value = string.Empty
                    }).ToList(),
                    Sites = rl.CheckListAttributeMap
                    .Where(r => r.CheckListAttributeId == siteAttribute.CheckListAttributeId)
                    .Select(g => new KeyValueModel
                    {
                        Key = g.CheckListAttributeValueId,
                        Value = string.Empty
                    }).ToList()
                }).ToList();

                return checklist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
