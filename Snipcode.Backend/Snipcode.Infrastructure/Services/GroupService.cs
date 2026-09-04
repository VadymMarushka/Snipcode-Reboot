using Microsoft.EntityFrameworkCore;
using Snipcode.Application.DTOs.Common;
using Snipcode.Application.DTOs.Groups;
using Snipcode.Application.DTOs.Snippets;
using Snipcode.Application.Interfaces;
using Snipcode.Application.Mappings;
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
    public async Task<PagedResultDto<GroupResponseDto>> GetPublicGroupsAsync(GroupQueryDto query, CancellationToken ct = default)
    {
        var baseQuery = _dbContext.SnippetGroups
            .Include(g => g.Owner)
            .Include(g => g.Snippets)
            .Where(g => g.IsPublic)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var term = query.SearchTerm.Trim().ToLower();
            baseQuery = baseQuery.Where(g =>
                g.Name.ToLower().Contains(term) ||
                (g.Description != null && g.Description.ToLower().Contains(term)));
        }

        if (query.Category.HasValue)
        {
            baseQuery = baseQuery.Where(g => g.Category == query.Category.Value);
        }

        if (query.Technologies != null && query.Technologies.Any())
        {
            baseQuery = baseQuery.Where(g => g.Snippets.Any(s => query.Technologies.Contains(s.Technology)));
        }

        baseQuery = query.SortBy?.ToLower() switch
        {
            "oldest" => baseQuery.OrderBy(g => g.CreatedAt),
            _ => baseQuery.OrderByDescending(g => g.CreatedAt), // "latest" or null/unrecognized
        };

        var totalCount = await baseQuery.CountAsync(ct);
        var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
        var pageSize = query.PageSize is < 1 or > 50 ? 10 : query.PageSize;

        var groupEntities = await baseQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var groups = groupEntities.Select(g => g.ToResponseDto()).ToList();

        return new PagedResultDto<GroupResponseDto>(groups, totalCount, pageNumber, pageSize);
    }

    public async Task<PagedResultDto<GroupResponseDto>> GetMyGroupsAsync(Guid userId, GroupQueryDto query, CancellationToken ct = default)
    {
        var baseQuery = _dbContext.SnippetGroups
            .Include(g => g.Owner)
            .Include(g => g.Snippets)
            .Where(g => g.OwnerId == userId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var term = query.SearchTerm.Trim().ToLower();
            baseQuery = baseQuery.Where(g =>
                g.Name.ToLower().Contains(term) ||
                (g.Description != null && g.Description.ToLower().Contains(term)));
        }

        if (query.Category.HasValue)
        {
            baseQuery = baseQuery.Where(g => g.Category == query.Category.Value);
        }

        if (query.Technologies != null && query.Technologies.Any())
        {
            baseQuery = baseQuery.Where(g => g.Snippets.Any(s => query.Technologies.Contains(s.Technology)));
        }

        baseQuery = query.SortBy?.ToLower() switch
        {
            "oldest" => baseQuery.OrderBy(g => g.CreatedAt),
            _ => baseQuery.OrderByDescending(g => g.CreatedAt), // "latest" or null/unrecognized
        };

        var totalCount = await baseQuery.CountAsync(ct);
        var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
        var pageSize = query.PageSize is < 1 or > 50 ? 10 : query.PageSize;

        var groupEntities = await baseQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var groups = groupEntities.Select(g => g.ToResponseDto()).ToList();

        return new PagedResultDto<GroupResponseDto>(groups, totalCount, pageNumber, pageSize);
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

        return group.ToResponseDto();
    }

    public async Task<GroupDetailResponseDto> GetByIdAsync(Guid id, Guid? currentUserId, CancellationToken ct = default)
    {
        var group = await _dbContext.SnippetGroups
            .Include(g => g.Owner)
            .Include(g => g.Snippets).ThenInclude(s => s.Author)
            .Include(g => g.Snippets).ThenInclude(s => s.SnippetTags).ThenInclude(st => st.Tag)
            .FirstOrDefaultAsync(g => g.Id == id, ct);

        if (group == null)
            throw new KeyNotFoundException("Group was not found.");

        if (!group.IsPublic && group.OwnerId != currentUserId)
            throw new UnauthorizedAccessException("You do not have access to this private group.");

        var snippetDtos = new List<SnippetResponseDto>();
        foreach (var snippet in group.Snippets)
        {
            var content = await _blobStorage.GetSnippetContentAsync(snippet.BlobKey, ct);
            snippetDtos.Add(snippet.ToResponseDto(content));
        }

        return group.ToDetailResponseDto(snippetDtos);
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

        return group.ToResponseDto();
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