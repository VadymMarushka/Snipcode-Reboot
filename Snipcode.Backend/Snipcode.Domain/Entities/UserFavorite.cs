namespace Snipcode.Domain.Entities;

public class UserFavoriteSnippet
{
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;

    public Guid SnippetId { get; set; }
    public Snippet Snippet { get; set; } = null!;

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}