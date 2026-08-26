using Snipcode.Domain.Enums;

namespace Snipcode.Application.DTOs.Snippets;

public record SnippetResponseDto(
    Guid Id,
    string Title,
    string? Description,
    Technology Technology,
    string CodeContent,
    bool IsPublic,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    Guid AuthorId,
    string AuthorUsername,
    Guid? GroupId,
    List<string> Tags
);