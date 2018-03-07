namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public enum SaveStrategy
    {
        // In a case of conflict, overwrite database version with changes
        ClientFirst,

        // In case of a conflict, changes will be discarded and the database version will be used
        DatabaseFirst
    }
}