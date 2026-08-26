using System.ComponentModel.DataAnnotations;

namespace Snipcode.Application.DTOs.Auth;

public record RefreshTokenRequestDto(
    [Required(ErrorMessage = "AccessToken is required.")] string AccessToken,
    [Required(ErrorMessage = "RefreshToken is required.")] string RefreshToken
);