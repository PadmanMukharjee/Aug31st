using ConfigManager;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace M3Pact.Infrastructure.Common
{
    public static class Helper
    {
        /// <summary>
        /// Gives random token number
        /// </summary>
        /// <param name="tokenLength"></param>
        /// <returns></returns>
        public static string GenerateRandomToken(int tokenLength = BusinessConstants.DEFAULT_TOKEN_LEN)
        {
            return Guid.NewGuid().ToString(BusinessConstants.NUMERIC).Substring(0, tokenLength);
        }

        /// <summary>
        /// Gets Hashed QuoteId for given quote Id
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        public static string GetHashedValue(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return HashingUtility.Base64Encode(key);
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets string quoteId from hashed quote id
        /// </summary>
        /// <param name="hashedQuoteId"></param>
        /// <returns></returns>
        public static string GetDeHashedKey(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return HashingUtility.Base64Decode(value);
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns value for the given key from appsettings.json
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigurationKey(string key)
        {
            IConfigurationProvider config = new ConfigurationProvider();
            return config.GetValue(key);
        }

        /// <summary>
        /// Convert Array To Pivot Array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TColumn"></typeparam>
        /// <typeparam name="TRow"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <param name="source"></param>
        /// <param name="columnSelector"></param>
        /// <param name="rowSelector"></param>
        /// <param name="dataSelector"></param>
        /// <returns></returns>
        public static List<IDictionary<string, object>> ToPivotArray<T, TColumn, TRow, TData>(this IEnumerable<T> source,
                                                                                                Func<T, TColumn> columnSelector,
                                                                                                Expression<Func<T, TRow>> rowSelector,
                                                                                                Func<IEnumerable<T>, TData> dataSelector)
        {
            var array = new List<IDictionary<string, object>>();
            var cols = new List<string>();
            String rowName = ((MemberExpression)rowSelector.Body).Member.Name;
            var columns = source.Select(columnSelector).Distinct();
            cols = (new[] { rowName }).Concat(columns.Select(x => x.ToString())).ToList();

            var rows = source.OrderBy(x => rowSelector).GroupBy(rowSelector.Compile())
                             .Select(rowGroup => new
                             {
                                 rowGroup.Key,
                                 Values = columns.GroupJoin(
                                     rowGroup,
                                     c => c,
                                     r => columnSelector(r), 
                                     (c, columnGroup) => dataSelector(columnGroup))
                             }).ToArray();

            foreach (var row in rows)
            {
                var items = row.Values.Cast<object>().ToList();
                items.Insert(0, row.Key);
                var obj = GetAnonymousObject(cols, items);           
                array.Add(obj);
            }
            return array;
        }

        /// <summary>
        /// Get Anonymous Object 
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private static IDictionary<string, object> GetAnonymousObject(IEnumerable<string> columns, IEnumerable<object> values)
        {
            IDictionary<string, object> expandoObject = new ExpandoObject() as IDictionary<string, object>;
            int i;
            for (i = 0; i < columns.Count(); i++)
            {
                expandoObject.Add(columns.ElementAt<string>(i), values.ElementAt<object>(i));
            }
            return expandoObject;
        }

    }
}
