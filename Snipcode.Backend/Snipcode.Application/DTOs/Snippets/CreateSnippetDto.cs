using Snipcode.Domain.Enums;

namespace Snipcode.Application.DTOs.Snippets;

public record CreateSnippetDto(
    string Title,
    string? Description,
    Technology Technology,
    string CodeContent,
    bool IsPublic,
    Guid? GroupId,
    List<string>? Tags
);