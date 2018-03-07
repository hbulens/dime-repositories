using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Dime.Repositories
{
    internal class LinqOrderHelper<TSource>
    {
        #region Constructor

        internal LinqOrderHelper(string methodName, string propertyName)
        {
            this.Method = methodName;
            this.ParentParameterExpression = Expression.Parameter(typeof(TSource), "x");
            this.MemberExpression = this.SetMember(this.Parse(propertyName));
        }

        #endregion Constructor

        #region Properties

        private IOrderedEnumerable<KeyValuePair<int, string>> Properties { get; set; }
        private string Method { get; set; }
        private ParameterExpression ParentParameterExpression { get; set; }
        private MemberExpression MemberExpression { get; set; }

        internal IOrderedQueryable<TSource> GetAsQueryable(IEnumerable<TSource> query)
        {
            LambdaExpression selector = Expression.Lambda(this.MemberExpression, new ParameterExpression[] { this.ParentParameterExpression });
            MethodInfo methodInfo = this.GetMethodInfo(this.Method, this.MemberExpression.Type);

            var newQuery = (IOrderedQueryable<TSource>)methodInfo.Invoke(methodInfo, new object[] { query, selector });
            return newQuery;
        }

        internal IOrderedEnumerable<TSource> GetAsEnumerable(IEnumerable<TSource> query)
        {
            LambdaExpression selector = Expression.Lambda(this.MemberExpression, new ParameterExpression[] { this.ParentParameterExpression });
            MethodInfo methodInfo = this.GetMethodInfo(this.Method, this.MemberExpression.Type);

            var newQuery = (IOrderedEnumerable<TSource>)methodInfo.Invoke(methodInfo, new object[] { query, selector });
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
                if (i == 0)
                {
                    memberExpression = Expression.PropertyOrField(this.ParentParameterExpression, dataIndex);
                }
                else
                {
                    memberExpression = Expression.PropertyOrField(memberExpression, dataIndex);
                }
            }

            return memberExpression;
        }

        private MethodInfo GetMethodInfo(string methodName, Type type)
        {
            //Get System.Linq.Queryable.OrderBy() method.
            var enumarableType = typeof(System.Linq.Queryable);
            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == methodName && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     //Put more restriction here to ensure selecting the right overload
                     return parameters.Count == 2;//overload that has 2 parameters
                 }).Single();

            //The linq's OrderBy<T, TKey> has two generic types, which provided here
            MethodInfo genericMethod = method.MakeGenericMethod(typeof(TSource), type);

            return genericMethod;
        }

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