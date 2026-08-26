using Microsoft.EntityFrameworkCore;
using Snipcode.Application.DTOs.Groups;
using Snipcode.Application.DTOs.Snippets;
using Snipcode.Application.Interfaces;
using Snipcode.Domain.Entities;
using Snipcode.Infrastructure.Data;

namespace Snipcode.Infrastructure.Services;

public class GroupService : IGroupService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IBlobStorageService _blobStorage;

    public GroupService(ApplicationDbContext dbContext, IBlobStorageService blobStorage)
    {
        _dbContext = dbContext;
        _blobStorage = blobStorage;
    }

    public async Task<GroupResponseDto> CreateAsync(CreateGroupDto dto, Guid userId, CancellationToken ct = default)
    {
        var group = new SnippetGroup
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Category = dto.Category,
            IsPublic = dto.IsPublic,
            OwnerId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.SnippetGroups.Add(group);
        await _dbContext.SaveChangesAsync(ct);

        return new GroupResponseDto(group.Id, group.Name, group.Description, group.Category, group.IsPublic, group.CreatedAt, group.OwnerId, 0);
    }

    public async Task<GroupDetailResponseDto> GetByIdAsync(Guid id, Guid? currentUserId, CancellationToken ct = default)
    {
        var group = await _dbContext.SnippetGroups
            .Include(g => g.Snippets)
                .ThenInclude(s => s.Author)
            .Include(g => g.Snippets)
                .ThenInclude(s => s.SnippetTags)
                    .ThenInclude(st => st.Tag)
            .FirstOrDefaultAsync(g => g.Id == id, ct);

        if (group == null)
            throw new KeyNotFoundException("Group was not found.");

        if (!group.IsPublic && group.OwnerId != currentUserId)
            throw new UnauthorizedAccessException("You do not have access to this private group.");

        var snippetDtos = new List<SnippetResponseDto>();
        foreach (var snippet in group.Snippets)
        {
            var content = await _blobStorage.GetSnippetContentAsync(snippet.BlobKey, ct);
            var tags = snippet.SnippetTags.Select(st => st.Tag.Name).ToList();

            snippetDtos.Add(new SnippetResponseDto(
                snippet.Id,
                snippet.Title,
                snippet.Description,
                snippet.Technology,
                content,
                snippet.IsPublic,
                snippet.CreatedAt,
                snippet.UpdatedAt,
                snippet.AuthorId,
                snippet.Author.UserName!,
                snippet.GroupId,
                tags
            ));
        }

        return new GroupDetailResponseDto(
            group.Id,
            group.Name,
            group.Description,
            group.Category,
            group.IsPublic,
            group.CreatedAt,
            group.OwnerId,
            snippetDtos
        );
    }

    public async Task<IEnumerable<GroupResponseDto>> GetMyGroupsAsync(Guid userId, CancellationToken ct = default)
    {
        return await _dbContext.SnippetGroups
            .Where(g => g.OwnerId == userId)
            .Select(g => new GroupResponseDto(
                g.Id,
                g.Name,
                g.Description,
                g.Category,
                g.IsPublic,
                g.CreatedAt,
                g.OwnerId,
                g.Snippets.Count
            ))
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<GroupResponseDto> UpdateAsync(Guid id, UpdateGroupDto dto, Guid userId, CancellationToken ct = default)
    {
        var group = await _dbContext.SnippetGroups
            .Include(g => g.Snippets)
            .FirstOrDefaultAsync(g => g.Id == id, ct);

        if (group == null)
            throw new KeyNotFoundException("Group was not found.");

        if (group.OwnerId != userId)
            throw new UnauthorizedAccessException("You can only edit your own groups.");

        group.Name = dto.Name;
        group.Description = dto.Description;
        group.Category = dto.Category;
        group.IsPublic = dto.IsPublic;

        await _dbContext.SaveChangesAsync(ct);

        return new GroupResponseDto(group.Id, group.Name, group.Description, group.Category, group.IsPublic, group.CreatedAt, group.OwnerId, group.Snippets.Count);
    }

    public async Task DeleteAsync(Guid id, Guid userId, CancellationToken ct = default)
    {
        var group = await _dbContext.SnippetGroups
            .Include(g => g.Snippets)
            .FirstOrDefaultAsync(g => g.Id == id, ct);

        if (group == null)
            throw new KeyNotFoundException("Group was not found.");

        if (group.OwnerId != userId)
            throw new UnauthorizedAccessException("You can only delete your own groups.");

        foreach (var snippet in group.Snippets)
        {
            snippet.GroupId = null;
        }

        _dbContext.SnippetGroups.Remove(group);
        await _dbContext.SaveChangesAsync(ct);
    }
}