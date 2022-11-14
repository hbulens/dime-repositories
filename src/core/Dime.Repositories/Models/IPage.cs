namespace Dime.Repositories
{
    public interface IPage<T> : IResult<T>
    {
        int Total { get; set; }
    }
}