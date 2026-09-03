using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snipcode.API.Extensions;
using Snipcode.Application.DTOs.Groups;
using Snipcode.Application.Interfaces;
using System.Security.Claims;

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
        var result = await _groupService.CreateAsync(dto, User.GetUserId(), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        Guid? userId = User.Identity?.IsAuthenticated == true ? User.GetUserId() : null;
        var result = await _groupService.GetByIdAsync(id, userId, ct);
        return Ok(result);
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetMyGroups(CancellationToken ct)
    {
        var result = await _groupService.GetMyGroupsAsync(User.GetUserId(), ct);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGroupDto dto, CancellationToken ct)
    {
        var result = await _groupService.UpdateAsync(id, dto, User.GetUserId(), ct);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _groupService.DeleteAsync(id, User.GetUserId(), ct);
        return NoContent();
    }
}