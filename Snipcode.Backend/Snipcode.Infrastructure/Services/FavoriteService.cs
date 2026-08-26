using Microsoft.EntityFrameworkCore;
using Snipcode.Application.DTOs.Groups;
using Snipcode.Application.DTOs.Snippets;
using Snipcode.Application.Interfaces;
using Snipcode.Domain.Entities;
using Snipcode.Infrastructure.Data;

namespace Snipcode.Infrastructure.Services;

public class FavoriteService : IFavoriteService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IBlobStorageService _blobStorage;

    public FavoriteService(ApplicationDbContext dbContext, IBlobStorageService blobStorage)
    {
        _dbContext = dbContext;
        _blobStorage = blobStorage;
    }

    public async Task AddSnippetToFavoritesAsync(Guid snippetId, Guid userId, CancellationToken ct = default)
    {
        var snippet = await _dbContext.Snippets.FirstOrDefaultAsync(s => s.Id == snippetId, ct);
        if (snippet == null)
            throw new KeyNotFoundException("Snippet was not found.");

        if (!snippet.IsPublic && snippet.AuthorId != userId)
            throw new UnauthorizedAccessException("You cannot favorite a private snippet that is not yours.");

        var exists = await _dbContext.UserFavoriteSnippets
            .AnyAsync(f => f.UserId == userId && f.SnippetId == snippetId, ct);

        if (exists)
            throw new InvalidOperationException("Snippet is already in your favorites.");

        _dbContext.UserFavoriteSnippets.Add(new UserFavoriteSnippet
        {
            UserId = userId,
            SnippetId = snippetId,
            AddedAt = DateTime.UtcNow
        });

        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task RemoveSnippetFromFavoritesAsync(Guid snippetId, Guid userId, CancellationToken ct = default)
    {
        var favorite = await _dbContext.UserFavoriteSnippets
            .FirstOrDefaultAsync(f => f.UserId == userId && f.SnippetId == snippetId, ct);

        if (favorite == null)
            throw new KeyNotFoundException("Snippet is not in your favorites.");

        _dbContext.UserFavoriteSnippets.Remove(favorite);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<SnippetResponseDto>> GetFavoriteSnippetsAsync(Guid userId, CancellationToken ct = default)
    {
        var favorites = await _dbContext.UserFavoriteSnippets
            .Include(f => f.Snippet)
                .ThenInclude(s => s.Author)
            .Include(f => f.Snippet)
                .ThenInclude(s => s.SnippetTags)
                    .ThenInclude(st => st.Tag)
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.AddedAt)
            .Select(f => f.Snippet)
            .ToListAsync(ct);

        var result = new List<SnippetResponseDto>();
        foreach (var snippet in favorites)
        {
            var codeContent = await _blobStorage.GetSnippetContentAsync(snippet.BlobKey, ct);
            var tags = snippet.SnippetTags.Select(st => st.Tag.Name).ToList();

            result.Add(new SnippetResponseDto(
                snippet.Id,
                snippet.Title,
                snippet.Description,
                snippet.Technology,
                codeContent,
                snippet.IsPublic,
                snippet.CreatedAt,
                snippet.UpdatedAt,
                snippet.AuthorId,
                snippet.Author.UserName!,
                snippet.GroupId,
                tags
            ));
        }

        return result;
    }

    public async Task AddGroupToFavoritesAsync(Guid groupId, Guid userId, CancellationToken ct = default)
    {
        var group = await _dbContext.SnippetGroups.FirstOrDefaultAsync(g => g.Id == groupId, ct);
        if (group == null)
            throw new KeyNotFoundException("Group was not found.");

        if (!group.IsPublic && group.OwnerId != userId)
            throw new UnauthorizedAccessException("You cannot favorite a private group that is not yours.");

        var exists = await _dbContext.UserFavoriteGroups
            .AnyAsync(f => f.UserId == userId && f.GroupId == groupId, ct);

        if (exists)
            throw new InvalidOperationException("Group is already in your favorites.");

        _dbContext.UserFavoriteGroups.Add(new UserFavoriteGroup
        {
            UserId = userId,
            GroupId = groupId,
            AddedAt = DateTime.UtcNow
        });

        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task RemoveGroupFromFavoritesAsync(Guid groupId, Guid userId, CancellationToken ct = default)
    {
        var favorite = await _dbContext.UserFavoriteGroups
            .FirstOrDefaultAsync(f => f.UserId == userId && f.GroupId == groupId, ct);

        if (favorite == null)
            throw new KeyNotFoundException("Group is not in your favorites.");

        _dbContext.UserFavoriteGroups.Remove(favorite);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<GroupResponseDto>> GetFavoriteGroupsAsync(Guid userId, CancellationToken ct = default)
    {
        return await _dbContext.UserFavoriteGroups
            .Include(f => f.Group)
                .ThenInclude(g => g.Snippets)
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.AddedAt)
            .Select(f => new GroupResponseDto(
                f.Group.Id,
                f.Group.Name,
                f.Group.Description,
                f.Group.Category,
                f.Group.IsPublic,
                f.Group.CreatedAt,
                f.Group.OwnerId,
                f.Group.Snippets.Count
            ))
            .ToListAsync(ct);
    }
}