using Snipcode.Domain.Enums;

namespace Snipcode.Domain.Entities;

public class SnippetGroup
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Category Category { get; set; }
    public bool IsPublic { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys & Navigation Properties
    public Guid OwnerId { get; set; }
    public ApplicationUser Owner { get; set; } = null!;

    public ICollection<Snippet> Snippets { get; set; } = new List<Snippet>();
    public ICollection<GroupTag> GroupTags { get; set; } = new List<GroupTag>();
    public ICollection<UserFavoriteGroup> FavoritedByUsers { get; set; } = new List<UserFavoriteGroup>();
}