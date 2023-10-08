namespace Dime.Repositories
{
    public class Order<T> : IOrder<T>
    {
        public Order(string property, bool isAscending)
        {
            Property = property;
            IsAscending = isAscending;
        }

        public string Property { get; set; }

        public bool IsAscending { get; set; }

        public void Deconstruct(out string property, out bool isAscending)
        {
            property = Property;
            isAscending = IsAscending;
        }
    }
}