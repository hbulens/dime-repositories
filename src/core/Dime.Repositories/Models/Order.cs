
namespace Dime.Repositories
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Order<T> : IOrder<T>
    {
        public Order(string property, bool isAscending)
        {
            Property = property;
            IsAscending = isAscending;
        }

        /// <summary>
        /// Gets or sets the sorting property
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Gets or sets the sorting direction
        /// </summary>
        public bool IsAscending { get; set; }

        public void Deconstruct(out string property, out bool isAscending)
        {
            property = Property;
            isAscending = IsAscending;
        }
    }
}