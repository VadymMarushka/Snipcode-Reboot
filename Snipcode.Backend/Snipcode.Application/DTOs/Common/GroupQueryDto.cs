using Snipcode.Domain.Enums;
using System.ComponentModel.DataAnnotations;

public record GroupQueryDto(
    [MaxLength(100, ErrorMessage = "Search term cannot exceed 100 characters.")]
    string? SearchTerm = null,
    Category? Category = null,
    IEnumerable<Technology>? Technologies = null,
    string? SortBy = "latest",
    [Range(1, int.MaxValue, ErrorMessage = "Page number must be at least 1.")]
    int PageNumber = 1,
    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
    int PageSize = 10
);