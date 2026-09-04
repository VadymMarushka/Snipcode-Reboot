using Microsoft.EntityFrameworkCore;
using Snipcode.Application.DTOs.Common;
using Snipcode.Application.DTOs.Snippets;
using Snipcode.Application.Interfaces;
using Snipcode.Application.Mappings;
using Snipcode.Domain.Entities;
using Snipcode.Infrastructure.Data;

namespace Snipcode.Infrastructure.Services;

public class SnippetService : ISnippetService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IBlobStorageService _blobStorage;

    public SnippetService(ApplicationDbContext dbContext, IBlobStorageService blobStorage)
    {
        _dbContext = dbContext;
        _blobStorage = blobStorage;
    }

    public async Task<SnippetResponseDto> CreateAsync(CreateSnippetDto dto, Guid userId, CancellationToken ct = default)
    {
        var blobKey = $"{userId}/{Guid.NewGuid()}.txt";
        await _blobStorage.UploadSnippetAsync(blobKey, dto.CodeContent, ct);

        try
        {
            var snippet = new Snippet
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Technology = dto.Technology,
                BlobKey = blobKey,
                IsPublic = dto.IsPublic,
                AuthorId = userId,
                GroupId = dto.GroupId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            if (dto.Tags != null && dto.Tags.Count > 0)
            {
                await HandleTagsAsync(snippet, dto.Tags, ct);
            }

            _dbContext.Snippets.Add(snippet);
            await _dbContext.SaveChangesAsync(ct);

            // Підвантажуємо групу та автора для маппінгу
            await _dbContext.Entry(snippet).Reference(s => s.Author).LoadAsync(ct);
            if (snippet.GroupId.HasValue)
            {
                await _dbContext.Entry(snippet).Reference(s => s.Group).LoadAsync(ct);
            }

            return snippet.ToResponseDto(dto.CodeContent);
        }
        catch
        {
            await _blobStorage.DeleteSnippetAsync(blobKey, ct);
            throw;
        }
    }

    public async Task<SnippetResponseDto> GetByIdAsync(Guid id, Guid? currentUserId, CancellationToken ct = default)
    {
        var snippet = await _dbContext.Snippets
            .Include(s => s.Author)
            .Include(s => s.Group)
            .Include(s => s.SnippetTags).ThenInclude(st => st.Tag)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        if (snippet == null)
            throw new KeyNotFoundException("Snippet was not found.");

        if (!snippet.IsPublic && snippet.AuthorId != currentUserId)
            throw new UnauthorizedAccessException("You do not have access to this private snippet.");

        var codeContent = await _blobStorage.GetSnippetContentAsync(snippet.BlobKey, ct);
        return snippet.ToResponseDto(codeContent);
    }
    public async Task<PagedResultDto<SnippetResponseDto>> GetPublicSnippetsAsync(SnippetQueryDto query, CancellationToken ct = default)
    {
        var baseQuery = _dbContext.Snippets
            .Include(s => s.Author)
            .Include(s => s.Group)
            .Include(s => s.SnippetTags).ThenInclude(st => st.Tag)
            .Where(s => s.IsPublic)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var term = query.SearchTerm.Trim().ToLower();
            baseQuery = baseQuery.Where(s =>
                s.Title.ToLower().Contains(term) ||
                (s.Description != null && s.Description.ToLower().Contains(term)));
        }

        if (query.Category.HasValue)
        {
            baseQuery = baseQuery.Where(s => s.Group != null && s.Group.Category == query.Category.Value);
        }

        if (query.Technologies != null && query.Technologies.Count > 0)
        {
            baseQuery = baseQuery.Where(s => query.Technologies.Contains(s.Technology));
        }

        if (!string.IsNullOrWhiteSpace(query.Tag))
        {
            var tag = query.Tag.Trim().ToLower();
            baseQuery = baseQuery.Where(s => s.SnippetTags.Any(st => st.Tag.Name == tag));
        }

        var totalCount = await baseQuery.CountAsync(ct);
        var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
        var pageSize = query.PageSize is < 1 or > 50 ? 12 : query.PageSize;

        baseQuery = query.SortBy?.ToLower() switch
        {
            "oldest" => baseQuery.OrderBy(s => s.CreatedAt),
            _ => baseQuery.OrderByDescending(s => s.CreatedAt)
        };

        var snippets = await baseQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var resultList = new List<SnippetResponseDto>();
        foreach (var snippet in snippets)
        {
            var codeContent = await _blobStorage.GetSnippetContentAsync(snippet.BlobKey, ct);
            resultList.Add(snippet.ToResponseDto(codeContent));
        }

        return new PagedResultDto<SnippetResponseDto>(resultList, totalCount, pageNumber, pageSize);
    }

    public async Task<SnippetStatsDto> GetPublicStatsAsync(CancellationToken ct = default)
    {
        var publicSnippets = _dbContext.Snippets.Where(s => s.IsPublic);

        var totalCount = await publicSnippets.CountAsync(ct);

        var techCounts = await publicSnippets
            .GroupBy(s => s.Technology)
            .Select(g => new { Tech = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Tech, x => x.Count, ct);

        var categoryCounts = await publicSnippets
            .Where(s => s.Group != null)
            .GroupBy(s => s.Group!.Category)
            .Select(g => new { Cat = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Cat, x => x.Count, ct);

        return new SnippetStatsDto(categoryCounts, techCounts, totalCount);
    }

    public async Task<PagedResultDto<SnippetResponseDto>> GetMySnippetsAsync(Guid userId, SnippetQueryDto query, CancellationToken ct = default)
    {
        var baseQuery = _dbContext.Snippets
            .Include(s => s.Author)
            .Include(s => s.Group)
            .Include(s => s.SnippetTags).ThenInclude(st => st.Tag)
            .Where(s => s.AuthorId == userId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var term = query.SearchTerm.Trim().ToLower();
            baseQuery = baseQuery.Where(s =>
                s.Title.ToLower().Contains(term) ||
                (s.Description != null && s.Description.ToLower().Contains(term)));
        }

        if (query.Category.HasValue)
        {
            baseQuery = baseQuery.Where(s => s.Group != null && s.Group.Category == query.Category.Value);
        }

        if (query.Technologies != null && query.Technologies.Count > 0)
        {
            baseQuery = baseQuery.Where(s => query.Technologies.Contains(s.Technology));
        }

        if (!string.IsNullOrWhiteSpace(query.Tag))
        {
            var tag = query.Tag.Trim().ToLower();
            baseQuery = baseQuery.Where(s => s.SnippetTags.Any(st => st.Tag.Name == tag));
        }

        var totalCount = await baseQuery.CountAsync(ct);
        var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
        var pageSize = query.PageSize is < 1 or > 50 ? 12 : query.PageSize;

        baseQuery = query.SortBy?.ToLower() switch
        {
            "oldest" => baseQuery.OrderBy(s => s.CreatedAt),
            _ => baseQuery.OrderByDescending(s => s.CreatedAt)
        };

        var snippets = await baseQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var resultList = new List<SnippetResponseDto>();
        foreach (var snippet in snippets)
        {
            var codeContent = await _blobStorage.GetSnippetContentAsync(snippet.BlobKey, ct);
            resultList.Add(snippet.ToResponseDto(codeContent));
        }

        return new PagedResultDto<SnippetResponseDto>(resultList, totalCount, pageNumber, pageSize);
    }

    public async Task<SnippetResponseDto> UpdateAsync(Guid id, UpdateSnippetDto dto, Guid userId, CancellationToken ct = default)
    {
        var snippet = await _dbContext.Snippets
            .Include(s => s.Author)
            .Include(s => s.Group)
            .Include(s => s.SnippetTags).ThenInclude(st => st.Tag)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        if (snippet == null)
            throw new KeyNotFoundException("Snippet was not found.");

        if (snippet.AuthorId != userId)
            throw new UnauthorizedAccessException("You can only edit your own snippets.");

        snippet.Title = dto.Title;
        snippet.Description = dto.Description;
        snippet.Technology = dto.Technology;
        snippet.IsPublic = dto.IsPublic;
        snippet.GroupId = dto.GroupId;
        snippet.UpdatedAt = DateTime.UtcNow;

        await _blobStorage.UploadSnippetAsync(snippet.BlobKey, dto.CodeContent, ct);

        snippet.SnippetTags.Clear();
        if (dto.Tags != null && dto.Tags.Count > 0)
        {
            await HandleTagsAsync(snippet, dto.Tags, ct);
        }

        await _dbContext.SaveChangesAsync(ct);
        return snippet.ToResponseDto(dto.CodeContent);
    }

    public async Task DeleteAsync(Guid id, Guid userId, CancellationToken ct = default)
    {
        var snippet = await _dbContext.Snippets.FirstOrDefaultAsync(s => s.Id == id, ct);

        if (snippet == null)
            throw new KeyNotFoundException("Snippet was not found.");

        if (snippet.AuthorId != userId)
            throw new UnauthorizedAccessException("You can only delete your own snippets.");

        await _blobStorage.DeleteSnippetAsync(snippet.BlobKey, ct);
        _dbContext.Snippets.Remove(snippet);
        await _dbContext.SaveChangesAsync(ct);
    }

    private async Task HandleTagsAsync(Snippet snippet, List<string> tagNames, CancellationToken ct)
    {
        foreach (var name in tagNames.Select(t => t.Trim().ToLower()).Distinct())
        {
            var tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Name == name, ct);
            if (tag == null)
            {
                tag = new Tag { Id = Guid.NewGuid(), Name = name };
                _dbContext.Tags.Add(tag);
            }

            snippet.SnippetTags.Add(new SnippetTag { Snippet = snippet, Tag = tag });
        }
    }
}