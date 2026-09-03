using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snipcode.API.Extensions;
using Snipcode.Application.Interfaces;
using System.Security.Claims;

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
        await _favoriteService.AddSnippetToFavoritesAsync(snippetId, User.GetUserId(), ct);
        return Ok(new { Message = "Snippet added to favorites." });
    }

    [HttpDelete("snippets/{snippetId:guid}")]
    public async Task<IActionResult> RemoveSnippet(Guid snippetId, CancellationToken ct)
    {
        await _favoriteService.RemoveSnippetFromFavoritesAsync(snippetId, User.GetUserId(), ct);
        return NoContent();
    }

    [HttpGet("snippets")]
    public async Task<IActionResult> GetFavoriteSnippets(CancellationToken ct)
    {
        var result = await _favoriteService.GetFavoriteSnippetsAsync(User.GetUserId(), ct);
        return Ok(result);
    }

    [HttpPost("groups/{groupId:guid}")]
    public async Task<IActionResult> AddGroup(Guid groupId, CancellationToken ct)
    {
        await _favoriteService.AddGroupToFavoritesAsync(groupId, User.GetUserId(), ct);
        return Ok(new { Message = "Group added to favorites." });
    }

    [HttpDelete("groups/{groupId:guid}")]
    public async Task<IActionResult> RemoveGroup(Guid groupId, CancellationToken ct)
    {
        await _favoriteService.RemoveGroupFromFavoritesAsync(groupId, User.GetUserId(), ct);
        return NoContent();
    }

    [HttpGet("groups")]
    public async Task<IActionResult> GetFavoriteGroups(CancellationToken ct)
    {
        var result = await _favoriteService.GetFavoriteGroupsAsync(User.GetUserId(), ct);
        return Ok(result);
    }
}