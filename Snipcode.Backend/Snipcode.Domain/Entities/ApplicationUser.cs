using Microsoft.AspNetCore.Identity;

namespace Snipcode.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Snippet> Snippets { get; set; } = new List<Snippet>();
    public ICollection<SnippetGroup> Groups { get; set; } = new List<SnippetGroup>();
    public ICollection<UserFavoriteSnippet> FavoriteSnippets { get; set; } = new List<UserFavoriteSnippet>();
    public ICollection<UserFavoriteGroup> FavoriteGroups { get; set; } = new List<UserFavoriteGroup>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}