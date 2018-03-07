namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public enum ConcurrencyStrategy
    {
        // In a case of conflict, overwrite database version with changes
        ClientFirst,

        // In case of a conflict, changes will be discarded and the database version will be used
        DatabaseFirst
    }
}