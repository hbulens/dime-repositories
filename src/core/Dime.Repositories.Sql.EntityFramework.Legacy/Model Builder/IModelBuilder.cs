using System.Data.Entity;

namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IModelBuilder<T> where T : DbContext
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        void BuildContext(DbModelBuilder builder);

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="schema"></param>
        void BuildContext(DbModelBuilder builder, string schema);
    }
}