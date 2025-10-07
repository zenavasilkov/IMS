namespace Shared.Pagination
{
    public record PagedList<T>(List<T> Items, int PageNumber, int PageSize, int PageCount, int PageTotal)
    {
        public bool HasPrevious => PageNumber > 1;

        public bool HasNext => PageNumber < PageCount;
    }
}
