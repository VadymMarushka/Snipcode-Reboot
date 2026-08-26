using Snipcode.Application.DTOs.Groups;
using Snipcode.Application.DTOs.Snippets;

namespace Snipcode.Application.Interfaces;

public interface IFavoriteService
{
    Task AddSnippetToFavoritesAsync(Guid snippetId, Guid userId, CancellationToken ct = default);
    Task RemoveSnippetFromFavoritesAsync(Guid snippetId, Guid userId, CancellationToken ct = default);
    Task<IEnumerable<SnippetResponseDto>> GetFavoriteSnippetsAsync(Guid userId, CancellationToken ct = default);

    Task AddGroupToFavoritesAsync(Guid groupId, Guid userId, CancellationToken ct = default);
    Task RemoveGroupFromFavoritesAsync(Guid groupId, Guid userId, CancellationToken ct = default);
    Task<IEnumerable<GroupResponseDto>> GetFavoriteGroupsAsync(Guid userId, CancellationToken ct = default);
}