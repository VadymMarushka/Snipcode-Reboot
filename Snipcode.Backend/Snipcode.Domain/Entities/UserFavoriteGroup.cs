namespace Snipcode.Domain.Entities;

public class UserFavoriteGroup
{
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;

    public Guid GroupId { get; set; }
    public SnippetGroup Group { get; set; } = null!;

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}