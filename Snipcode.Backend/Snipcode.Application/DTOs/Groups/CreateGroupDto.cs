using System.ComponentModel.DataAnnotations;
using Snipcode.Domain.Enums;

namespace Snipcode.Application.DTOs.Groups;

public record CreateGroupDto(
    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Group name must be between 2 and 100 characters.")]
    string Name,

    [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    string? Description,

    [Required]
    [EnumDataType(typeof(Category), ErrorMessage = "Invalid category value.")]
    Category Category,

    bool IsPublic = false
);