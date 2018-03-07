using System.Collections.Generic;
using System.Linq;

namespace Dime.Repositories
{
    public static class OrderLinq
    {
        private static IOrderedEnumerable<KeyValuePair<int, string>> GetDataIndex(string propertyName)
        {
            int c = 1;
            IDictionary<int, string> properties = new Dictionary<int, string>();
            propertyName.Split('.').ToList().ForEach(x => { properties.Add(c, x); c++; });

            IOrderedEnumerable<KeyValuePair<int, string>> orderedList = properties.OrderBy(x => x.Key);
            return orderedList;
        }

        public static IOrderedQueryable<T> OrderDescending<T>(this IEnumerable<T> query, string propertyName)
        {
            LinqOrderHelper<T> helper = new LinqOrderHelper<T>("OrderByDescending", propertyName);
            return helper.GetAsQueryable(query);
        }

        public static IOrderedQueryable<T> Order<T>(this IEnumerable<T> query, string propertyName)
        {
            LinqOrderHelper<T> helper = new LinqOrderHelper<T>("OrderBy", propertyName);
            return helper.GetAsQueryable(query);
        }

        public static IOrderedQueryable<TSource> ThenBy<TSource>(this IEnumerable<TSource> source, string property)
        {
            LinqOrderHelper<TSource> helper = new LinqOrderHelper<TSource>("ThenBy", property);
            return helper.GetAsQueryable(source);
        }

        public static IOrderedQueryable<TSource> ThenByDescending<TSource>(this IEnumerable<TSource> source, string property)
        {
            LinqOrderHelper<TSource> helper = new LinqOrderHelper<TSource>("ThenByDescending", property);
            return helper.GetAsQueryable(source);
        }
    }
}