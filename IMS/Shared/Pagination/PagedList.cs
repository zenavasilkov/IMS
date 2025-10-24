namespace Shared.Pagination;

public record PagedList<T>(List<T> Items, int PageNumber, int PageSize, int TotalCount)
{
    public bool HasPrevious => PageNumber > 1;

    public bool HasNext => PageSize > 0 && PageNumber < Math.Ceiling((double)(TotalCount / PageSize));
}
