using Microsoft.EntityFrameworkCore;

namespace Dime.Repositories
{
    /// <summary>
    /// Represents a model builder for <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The db context type</typeparam>
    public interface IModelBuilder<T> where T : DbContext
    {
        /// <summary>
        /// Builds the default context
        /// </summary>
        /// <param name="builder">The code first model builder</param>
        void BuildContext(ModelBuilder builder);

        /// <summary>
        /// Builds the context for a schema other than the default dbo schema
        /// </summary>
        /// <param name="builder">The code first model builder</param>
        /// <param name="schema">The schema name</param>
        void BuildContext(ModelBuilder builder, string schema);
    }
}