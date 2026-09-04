using Snipcode.Application.DTOs.Common;
using Snipcode.Application.DTOs.Groups;

namespace Snipcode.Application.Interfaces;

public interface IGroupService
{
    Task<GroupResponseDto> CreateAsync(CreateGroupDto dto, Guid userId, CancellationToken ct = default);
    Task<GroupDetailResponseDto> GetByIdAsync(Guid id, Guid? currentUserId, CancellationToken ct = default);
    Task<PagedResultDto<GroupResponseDto>> GetPublicGroupsAsync(GroupQueryDto query, CancellationToken ct = default);
    Task<PagedResultDto<GroupResponseDto>> GetMyGroupsAsync(Guid userId, GroupQueryDto query, CancellationToken ct = default);
    Task<GroupResponseDto> UpdateAsync(Guid id, UpdateGroupDto dto, Guid userId, CancellationToken ct = default);
    Task DeleteAsync(Guid id, Guid userId, CancellationToken ct = default);
}