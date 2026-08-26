using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snipcode.Application.Interfaces;

namespace Snipcode.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FavoritesController : ControllerBase
{
    private readonly IFavoriteService _favoriteService;

    public FavoritesController(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    [HttpPost("snippets/{snippetId:guid}")]
    public async Task<IActionResult> AddSnippet(Guid snippetId, CancellationToken ct)
    {
        await _favoriteService.AddSnippetToFavoritesAsync(snippetId, GetCurrentUserId(), ct);
        return Ok(new { Message = "Snippet added to favorites." });
    }

    [HttpDelete("snippets/{snippetId:guid}")]
    public async Task<IActionResult> RemoveSnippet(Guid snippetId, CancellationToken ct)
    {
        await _favoriteService.RemoveSnippetFromFavoritesAsync(snippetId, GetCurrentUserId(), ct);
        return NoContent();
    }

    [HttpGet("snippets")]
    public async Task<IActionResult> GetFavoriteSnippets(CancellationToken ct)
    {
        var result = await _favoriteService.GetFavoriteSnippetsAsync(GetCurrentUserId(), ct);
        return Ok(result);
    }

    [HttpPost("groups/{groupId:guid}")]
    public async Task<IActionResult> AddGroup(Guid groupId, CancellationToken ct)
    {
        await _favoriteService.AddGroupToFavoritesAsync(groupId, GetCurrentUserId(), ct);
        return Ok(new { Message = "Group added to favorites." });
    }

    [HttpDelete("groups/{groupId:guid}")]
    public async Task<IActionResult> RemoveGroup(Guid groupId, CancellationToken ct)
    {
        await _favoriteService.RemoveGroupFromFavoritesAsync(groupId, GetCurrentUserId(), ct);
        return NoContent();
    }

    [HttpGet("groups")]
    public async Task<IActionResult> GetFavoriteGroups(CancellationToken ct)
    {
        var result = await _favoriteService.GetFavoriteGroupsAsync(GetCurrentUserId(), ct);
        return Ok(result);
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