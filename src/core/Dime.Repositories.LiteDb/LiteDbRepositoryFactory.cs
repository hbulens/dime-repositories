namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public class LiteDbRepositoryFactory : IRepositoryFactory
    {
        #region Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="connectionPath"></param>
        public LiteDbRepositoryFactory(string connectionPath)
        {
            this.ConnectionPath = connectionPath;
        }

        #endregion Constructor

        #region Properties

        public string ConnectionPath { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IRepository<T> Create<T>() where T : class, new()
        {
            return new LiteDbRepository<T>(this.ConnectionPath);
        }

        #endregion Methods
    }
}