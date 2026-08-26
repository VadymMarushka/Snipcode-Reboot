namespace Snipcode.Application.DTOs.Tags;

public record TagDto(
    Guid Id,
    string Name,
    int SnippetCount
);