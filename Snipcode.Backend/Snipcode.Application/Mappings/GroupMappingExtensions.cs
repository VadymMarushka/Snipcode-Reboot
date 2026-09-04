using Snipcode.Application.DTOs.Groups;
using Snipcode.Application.DTOs.Snippets;
using Snipcode.Domain.Entities;
using Snipcode.Domain.Enums;

namespace Snipcode.Application.Mappings;

public static class GroupMappingExtensions
{
    public static GroupResponseDto ToResponseDto(this SnippetGroup group)
    {
        return new GroupResponseDto(
            group.Id,
            group.Name,
            group.Description,
            group.Category,
            group.IsPublic,
            group.CreatedAt,
            group.OwnerId,
            group.Owner?.UserName ?? string.Empty,
            group.Snippets?.Count ?? 0,
            group.Snippets?.Select(s => s.Technology).Distinct() ?? Enumerable.Empty<Technology>()
        );
    }

    public static GroupDetailResponseDto ToDetailResponseDto(this SnippetGroup group, List<SnippetResponseDto> snippets)
    {
        return new GroupDetailResponseDto(
            group.Id,
            group.Name,
            group.Description,
            group.Category,
            group.IsPublic,
            group.CreatedAt,
            group.OwnerId,
            group.Owner?.UserName ?? string.Empty,
            snippets,
            group.Snippets?.Select(s => s.Technology).Distinct() ?? Enumerable.Empty<Technology>()
        );
    }
}