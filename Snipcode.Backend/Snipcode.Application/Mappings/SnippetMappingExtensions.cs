using Snipcode.Application.DTOs.Groups;
using Snipcode.Application.DTOs.Snippets;
using Snipcode.Domain.Entities;

namespace Snipcode.Application.Mappings;

public static class SnippetMappingExtensions
{
    public static GroupSummaryDto? ToGroupSummaryDto(this SnippetGroup? group)
    {
        if (group == null) return null;
        return new GroupSummaryDto(group.Id, group.Name, group.Category);
    }

    public static SnippetResponseDto ToResponseDto(this Snippet snippet, string codeContent)
    {
        var authorUsername = snippet.Author?.UserName ?? string.Empty;
        var tags = snippet.SnippetTags?.Select(st => st.Tag.Name).ToList() ?? new List<string>();

        return new SnippetResponseDto(
            snippet.Id,
            snippet.Title,
            snippet.Description,
            codeContent,
            snippet.Technology,
            snippet.IsPublic,
            snippet.CreatedAt,
            snippet.UpdatedAt,
            snippet.AuthorId,
            authorUsername,
            tags,
            snippet.Group.ToGroupSummaryDto()
        );
    }
}