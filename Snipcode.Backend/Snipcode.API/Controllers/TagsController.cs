using Microsoft.AspNetCore.Mvc;
using Snipcode.Application.Interfaces;

namespace Snipcode.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagsController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var tags = await _tagService.GetAllAsync(ct);
        return Ok(tags);
    }
}