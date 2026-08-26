using Snipcode.Domain.Enums;

namespace Snipcode.Application.DTOs.Snippets;

public record UpdateSnippetDto(
    string Title,
    string? Description,
    Technology Technology,
    string CodeContent,
    bool IsPublic,
    Guid? GroupId,
    List<string>? Tags
);