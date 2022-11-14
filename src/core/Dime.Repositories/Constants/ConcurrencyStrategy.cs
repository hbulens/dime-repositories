namespace Dime.Repositories
{
    public enum ConcurrencyStrategy
    {
        /// <summary>
        /// In a case of conflict, overwrite database version with changes
        /// </summary>
        ClientFirst,

        /// <summary>
        /// In case of a conflict, changes will be discarded and the database version will be used
        /// </summary>
        DatabaseFirst
    }
}