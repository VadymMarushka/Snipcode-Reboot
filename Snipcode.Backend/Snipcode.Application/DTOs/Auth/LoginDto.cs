using System.ComponentModel.DataAnnotations;

namespace Snipcode.Application.DTOs.Auth;

public record LoginDto(
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    [MaxLength(256)]
    string Email,

    [Required]
    [MaxLength(100)]
    string Password
);