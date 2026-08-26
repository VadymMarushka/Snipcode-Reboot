using Snipcode.Domain.Enums;

namespace Snipcode.Application.DTOs.Snippets;

public record SnippetQueryDto(
    string? SearchTerm = null,
    Technology? Technology = null,
    string? Tag = null,
    int PageNumber = 1,
    int PageSize = 10
);