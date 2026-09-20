namespace Application.Abstractions.Database.Types;

public record PaginationInfo(
    int PageIndex,
    int PageSize,
    int Total,
    int TotalPages,
    bool HasNext,
    bool HasPrevious
)
{
    public static readonly PaginationInfo Empty = new(0, 0, 0, 0, false, false);
}
