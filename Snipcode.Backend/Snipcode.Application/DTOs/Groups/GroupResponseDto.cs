using Snipcode.Domain.Enums;

namespace Snipcode.Application.DTOs.Groups;

public record GroupResponseDto(
    Guid Id,
    string Name,
    string? Description,
    Category Category,
    bool IsPublic,
    DateTime CreatedAt,
    Guid OwnerId,
    int SnippetCount
);