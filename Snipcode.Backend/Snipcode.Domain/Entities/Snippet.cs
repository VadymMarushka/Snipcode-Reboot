using Snipcode.Domain.Enums;

namespace Snipcode.Domain.Entities;

public class Snippet
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Technology Technology { get; set; }
    public string BlobKey { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys & Navigation Properties
    public Guid AuthorId { get; set; }
    public ApplicationUser Author { get; set; } = null!;

    public Guid? GroupId { get; set; }
    public SnippetGroup? Group { get; set; }

    public ICollection<SnippetTag> SnippetTags { get; set; } = new List<SnippetTag>();
    public ICollection<UserFavoriteSnippet> FavoritedByUsers { get; set; } = new List<UserFavoriteSnippet>();
}