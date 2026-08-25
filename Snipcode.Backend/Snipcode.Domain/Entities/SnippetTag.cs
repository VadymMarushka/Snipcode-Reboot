namespace Snipcode.Domain.Entities;

public class SnippetTag
{
    public Guid SnippetId { get; set; }
    public Snippet Snippet { get; set; } = null!;

    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}