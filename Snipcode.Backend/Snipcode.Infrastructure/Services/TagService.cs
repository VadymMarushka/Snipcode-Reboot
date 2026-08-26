using Microsoft.EntityFrameworkCore;
using Snipcode.Application.DTOs.Tags;
using Snipcode.Application.Interfaces;
using Snipcode.Infrastructure.Data;

namespace Snipcode.Infrastructure.Services;

public class TagService : ITagService
{
    private readonly ApplicationDbContext _dbContext;

    public TagService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TagDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbContext.Tags
            .Select(t => new TagDto(
                t.Id,
                t.Name,
                t.SnippetTags.Count
            ))
            .OrderByDescending(t => t.SnippetCount)
            .ToListAsync(ct);
    }
}