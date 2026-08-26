using System.ComponentModel.DataAnnotations;
using Snipcode.Domain.Enums;

namespace Snipcode.Application.DTOs.Groups;

public record UpdateGroupDto(
    [Required, MaxLength(100)] string Name,
    [MaxLength(500)] string? Description,
    Category Category,
    bool IsPublic
);