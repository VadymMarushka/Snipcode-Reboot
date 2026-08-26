namespace Snipcode.Application.DTOs.Common;

public record PagedResultDto<T>(
    IEnumerable<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize
)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}