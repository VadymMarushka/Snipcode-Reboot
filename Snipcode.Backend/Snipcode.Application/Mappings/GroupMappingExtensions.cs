using Snipcode.Application.DTOs.Groups;
using Snipcode.Application.DTOs.Snippets;
using Snipcode.Domain.Entities;

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
            group.Snippets?.Count ?? 0
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
            snippets
        );
    }
}