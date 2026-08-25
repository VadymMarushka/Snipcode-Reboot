namespace Snipcode.Domain.Entities;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<SnippetTag> SnippetTags { get; set; } = new List<SnippetTag>();
    public ICollection<GroupTag> GroupTags { get; set; } = new List<GroupTag>();
}