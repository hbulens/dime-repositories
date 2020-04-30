using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    [ExcludeFromCodeCoverage]
    internal class LinqOrderHelper<TSource>
    {
        #region Constructor

        internal LinqOrderHelper(string methodName, string propertyName)
        {
            Method = methodName;
            ParentParameterExpression = Expression.Parameter(typeof(TSource), "x");
            MemberExpression = SetMember(Parse(propertyName));
        }

        #endregion Constructor

        #region Properties

        private string Method { get; }
        private ParameterExpression ParentParameterExpression { get; }
        private MemberExpression MemberExpression { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        internal IOrderedQueryable<TSource> GetAsQueryable(IEnumerable<TSource> query)
        {
            LambdaExpression selector = Expression.Lambda(MemberExpression, ParentParameterExpression);
            MethodInfo methodInfo = GetMethodInfo(Method, MemberExpression.Type);

            IOrderedQueryable<TSource> newQuery = (IOrderedQueryable<TSource>)methodInfo.Invoke(methodInfo, new object[] { query, selector });
            return newQuery;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        internal IOrderedEnumerable<TSource> GetAsEnumerable(IEnumerable<TSource> query)
        {
            LambdaExpression selector = Expression.Lambda(MemberExpression, ParentParameterExpression);
            MethodInfo methodInfo = GetMethodInfo(Method, MemberExpression.Type);

            IOrderedEnumerable<TSource> newQuery = (IOrderedEnumerable<TSource>)methodInfo.Invoke(methodInfo, new object[] { query, selector });
            return newQuery;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="orderedList"></param>
        private MemberExpression SetMember(IOrderedEnumerable<KeyValuePair<int, string>> orderedList)
        {
            MemberExpression memberExpression = default(MemberExpression);
            for (int i = 0; i < orderedList.Count(); i++)
            {
                // Get the current record in the loop
                string dataIndex = orderedList.ElementAt(i).Value;

                // If this is the first iteration, just set the variable - else append the expa
                memberExpression = i == 0 ?
                    Expression.PropertyOrField(ParentParameterExpression, dataIndex) :
                    Expression.PropertyOrField(memberExpression, dataIndex);
            }

            return memberExpression;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private MethodInfo GetMethodInfo(string methodName, Type type)
        {
            //Get System.Linq.Queryable.OrderBy() method.
            Type enumarableType = typeof(Queryable);
            MethodInfo method = enumarableType.GetMethods()
                 .Where(m => m.Name == methodName && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     List<ParameterInfo> parameters = m.GetParameters().ToList();
                     //Put more restriction here to ensure selecting the right overload
                     return parameters.Count == 2;//overload that has 2 parameters
                 }).Single();

            //The linq's OrderBy<T, TKey> has two generic types, which provided here
            MethodInfo genericMethod = method.MakeGenericMethod(typeof(TSource), type);

            return genericMethod;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private IOrderedEnumerable<KeyValuePair<int, string>> Parse(string propertyName)
        {
            int c = 1;
            IDictionary<int, string> properties = new Dictionary<int, string>();
            propertyName.Split('.').ToList().ForEach(x => { properties.Add(c, x); c++; });

            IOrderedEnumerable<KeyValuePair<int, string>> orderedList = properties.OrderBy(x => x.Key);
            return orderedList;
        }

        #endregion Properties
    }
}