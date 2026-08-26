using Snipcode.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Snipcode.Application.DTOs.Snippets;

public record SnippetQueryDto(
    [MaxLength(100, ErrorMessage = "Search term cannot exceed 100 characters.")]
    string? SearchTerm = null,

    [EnumDataType(typeof(Technology), ErrorMessage = "Invalid technology value.")]
    Technology? Technology = null,

    [MaxLength(50, ErrorMessage = "Tag filter cannot exceed 50 characters.")]
    string? Tag = null,

    [Range(1, int.MaxValue, ErrorMessage = "Page number must be at least 1.")]
    int PageNumber = 1,

    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
    int PageSize = 10
);