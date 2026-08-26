using Snipcode.Application.DTOs.Snippets;
using Snipcode.Domain.Enums;

namespace Snipcode.Application.DTOs.Groups;

public record GroupDetailResponseDto(
    Guid Id,
    string Name,
    string? Description,
    Category Category,
    bool IsPublic,
    DateTime CreatedAt,
    Guid OwnerId,
    List<SnippetResponseDto> Snippets
);