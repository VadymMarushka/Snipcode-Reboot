namespace Snipcode.Domain.Entities;

public class GroupTag
{
    public Guid GroupId { get; set; }
    public SnippetGroup Group { get; set; } = null!;

    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}