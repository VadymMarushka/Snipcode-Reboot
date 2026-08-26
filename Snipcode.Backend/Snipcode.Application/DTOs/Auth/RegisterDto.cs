using System.ComponentModel.DataAnnotations;

namespace Snipcode.Application.DTOs.Auth;

public record RegisterDto(
    [Required] string Username,
    [Required, EmailAddress(ErrorMessage = "Invalid email address format.")] string Email,
    [Required, MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")] string Password
);