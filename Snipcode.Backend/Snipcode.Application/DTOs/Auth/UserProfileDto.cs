namespace Snipcode.Application.DTOs.Auth;

public record UserProfileDto(
    Guid Id,
    string Username,
    string Email,
    int SnippetCount,
    int GroupCount
);