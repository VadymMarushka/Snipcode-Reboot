namespace Snipcode.Application.DTOs.Auth;

public record AuthResponseDto(
    string AccessToken,
    string RefreshToken,
    string Username,
    string Email,
    DateTime AccessTokenExpiration
);