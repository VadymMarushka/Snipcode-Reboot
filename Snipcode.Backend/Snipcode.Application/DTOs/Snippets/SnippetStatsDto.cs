using Snipcode.Domain.Enums;

namespace Snipcode.Application.DTOs.Snippets;

public record SnippetStatsDto(
    Dictionary<Category, int> CategoryCounts,
    Dictionary<Technology, int> TechnologyCounts,
    int TotalCount
);