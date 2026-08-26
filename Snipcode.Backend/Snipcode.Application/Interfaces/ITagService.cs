using Snipcode.Application.DTOs.Tags;

namespace Snipcode.Application.Interfaces;

public interface ITagService
{
    Task<IEnumerable<TagDto>> GetAllAsync(CancellationToken ct = default);
}