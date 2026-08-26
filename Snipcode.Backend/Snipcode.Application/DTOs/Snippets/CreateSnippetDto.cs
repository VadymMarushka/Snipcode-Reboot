using Snipcode.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Snipcode.Application.DTOs.Snippets;

public record CreateSnippetDto(
    [Required]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 150 characters.")]
    string Title,

    [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    string? Description,

    [Required]
    [EnumDataType(typeof(Technology), ErrorMessage = "Invalid technology value.")]
    Technology Technology,

    [Required(ErrorMessage = "Code content cannot be empty.")]
    [MaxLength(100000, ErrorMessage = "Code content cannot exceed 100,000 characters.")]
    string CodeContent,

    bool IsPublic,

    Guid? GroupId,

    [MaxLength(10, ErrorMessage = "A snippet cannot have more than 10 tags.")]
    List<string>? Tags
);