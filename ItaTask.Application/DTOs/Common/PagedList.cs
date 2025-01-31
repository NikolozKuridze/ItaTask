namespace ItaTask.Application.DTOs.Common;

public record PagedList<T>
{
    public List<T> Items { get; init; } = new();
    public int TotalCount { get; init; }
    public int PageSize { get; init; }
    public int CurrentPage { get; init; }
}