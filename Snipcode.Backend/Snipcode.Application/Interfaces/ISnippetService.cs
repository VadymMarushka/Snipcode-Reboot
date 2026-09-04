using Snipcode.Application.DTOs.Common;
using Snipcode.Application.DTOs.Snippets;

namespace Snipcode.Application.Interfaces;

public interface ISnippetService
{
    Task<SnippetResponseDto> CreateAsync(CreateSnippetDto dto, Guid userId, CancellationToken ct = default);
    Task<SnippetResponseDto> GetByIdAsync(Guid id, Guid? currentUserId, CancellationToken ct = default);
    Task<PagedResultDto<SnippetResponseDto>> GetPublicSnippetsAsync(SnippetQueryDto query, CancellationToken ct = default);
    Task<PagedResultDto<SnippetResponseDto>> GetMySnippetsAsync(Guid userId, SnippetQueryDto query, CancellationToken ct = default);
    Task<SnippetStatsDto> GetPublicStatsAsync(CancellationToken ct = default);
    Task<SnippetResponseDto> UpdateAsync(Guid id, UpdateSnippetDto dto, Guid userId, CancellationToken ct = default);
    Task DeleteAsync(Guid id, Guid userId, CancellationToken ct = default);
}