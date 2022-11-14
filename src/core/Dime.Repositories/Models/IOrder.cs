namespace Dime.Repositories
{
    public interface IOrder<T>
    {
        bool IsAscending { get; set; }

        string Property { get; set; }

        void Deconstruct(out string property, out bool isAscending);
    }
}