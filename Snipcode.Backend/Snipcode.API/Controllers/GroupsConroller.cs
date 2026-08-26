using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snipcode.Application.DTOs.Groups;
using Snipcode.Application.Interfaces;

namespace Snipcode.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupsController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateGroupDto dto, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        var result = await _groupService.CreateAsync(dto, userId, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        Guid? userId = User.Identity?.IsAuthenticated == true ? GetCurrentUserId() : null;
        var result = await _groupService.GetByIdAsync(id, userId, ct);
        return Ok(result);
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetMyGroups(CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        var result = await _groupService.GetMyGroupsAsync(userId, ct);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGroupDto dto, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        var result = await _groupService.UpdateAsync(id, dto, userId, ct);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        await _groupService.DeleteAsync(id, userId, ct);
        return NoContent();
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                       ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User ID is missing or invalid in claims.");
        }
        return userId;
    }
}