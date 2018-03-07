using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    /// <history>
    /// [HB] 02/02/2016 - Create
    /// </history>
    public static class DbContextModelBuilder
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <history>
        /// [HB] 02/02/2016 - Refactor from two separate locations to prevent out-of-sync DbContext creation
        /// [HB] 10/02/2016 - Include tenant
        /// </history>
        public static void BuildModel<T>(this DbModelBuilder modelBuilder, string schema) where T : DbContext
        {
            Type foundType = Assembly.GetCallingAssembly().GetTypes().FirstOrDefault(x => typeof(IModelBuilder<T>).IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface);
            if (foundType == null)
                throw new ArgumentException(string.Format("No model builder found for context in assembly {0}", Assembly.GetCallingAssembly().FullName));

            object o = Activator.CreateInstance(foundType);
            IModelBuilder<T> concreteModelBuilder = (IModelBuilder<T>)o;
            concreteModelBuilder.BuildContext(modelBuilder, schema);
        }
    }
}