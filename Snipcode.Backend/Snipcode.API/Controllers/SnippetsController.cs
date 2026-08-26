using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snipcode.Application.DTOs.Snippets;
using Snipcode.Application.Interfaces;

namespace Snipcode.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SnippetsController : ControllerBase
{
    private readonly ISnippetService _snippetService;

    public SnippetsController(ISnippetService snippetService)
    {
        _snippetService = snippetService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateSnippetDto dto, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        var result = await _snippetService.CreateAsync(dto, userId, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        Guid? userId = User.Identity?.IsAuthenticated == true ? GetCurrentUserId() : null;
        var result = await _snippetService.GetByIdAsync(id, userId, ct);
        return Ok(result);
    }

    [HttpGet("public")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublic([FromQuery] SnippetQueryDto query, CancellationToken ct)
    {
        var result = await _snippetService.GetPublicSnippetsAsync(query, ct);
        return Ok(result);
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetMySnippets(CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        var result = await _snippetService.GetMySnippetsAsync(userId, ct);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSnippetDto dto, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        var result = await _snippetService.UpdateAsync(id, dto, userId, ct);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        await _snippetService.DeleteAsync(id, userId, ct);
        return NoContent();
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User ID is missing or invalid in claims.");
        }
        return userId;
    }
}