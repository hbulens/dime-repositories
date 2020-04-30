using System;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DbContextModelBuilder
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="schema"></param>
        public static void BuildModel<T>(this DbModelBuilder modelBuilder, string schema) where T : DbContext
        {
            Type foundType = Assembly.GetCallingAssembly()
                .GetTypes()
                .FirstOrDefault(x => typeof(IModelBuilder<T>).IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface);

            if (foundType == null)
                throw new ArgumentException($"No model builder found for context in assembly {0}", Assembly.GetCallingAssembly().FullName);

            object o = Activator.CreateInstance(foundType);
            IModelBuilder<T> concreteModelBuilder = (IModelBuilder<T>)o;
            concreteModelBuilder.BuildContext(modelBuilder, schema);
        }
    }
}