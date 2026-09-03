using Snipcode.Application.DTOs.Groups;
using Snipcode.Domain.Enums;

namespace Snipcode.Application.DTOs.Snippets;

public record SnippetResponseDto(
    Guid Id,
    string Title,
    string? Description,
    string CodeContent,
    Technology Technology,
    bool IsPublic,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    Guid AuthorId,
    string AuthorUsername,
    List<string> Tags,
    GroupSummaryDto? Group
);