using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Snipcode.Application.DTOs.Auth;
using Snipcode.Application.Interfaces;
using Snipcode.Domain.Entities;
using Snipcode.Infrastructure.Data;

namespace Snipcode.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ApplicationDbContext _dbContext;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IJwtTokenGenerator jwtTokenGenerator,
        ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _dbContext = dbContext;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new InvalidOperationException("User with this email already exists.");

        var user = new ApplicationUser
        {
            UserName = dto.Username,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Registration failed: {errors}");
        }

        return await GenerateAuthResponseAsync(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            throw new UnauthorizedAccessException("Invalid email or password.");

        return await GenerateAuthResponseAsync(user);
    }

    public async Task<UserProfileDto> GetProfileAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new KeyNotFoundException("User was not found.");

        var snippetCount = await _dbContext.Snippets.CountAsync(s => s.AuthorId == userId);
        var groupCount = await _dbContext.SnippetGroups.CountAsync(g => g.OwnerId == userId);

        return new UserProfileDto(user.Id, user.UserName!, user.Email!, snippetCount, groupCount);
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto)
    {
        var refreshTokenEntity = await _dbContext.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == dto.RefreshToken);

        if (refreshTokenEntity == null || !refreshTokenEntity.IsActive)
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");

        // Revoke current refresh token (rotation policy)
        refreshTokenEntity.RevokedAt = DateTime.UtcNow;

        return await GenerateAuthResponseAsync(refreshTokenEntity.User);
    }

    public async Task RevokeTokenAsync(string refreshToken)
    {
        var tokenEntity = await _dbContext.RefreshTokens.FirstOrDefaultAsync(r => r.Token == refreshToken);
        if (tokenEntity == null || !tokenEntity.IsActive)
            throw new InvalidOperationException("Token is invalid or already revoked.");

        tokenEntity.RevokedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
    }

    private async Task<AuthResponseDto> GenerateAuthResponseAsync(ApplicationUser user)
    {
        var (accessToken, expiration) = _jwtTokenGenerator.GenerateToken(user);
        var refreshToken = CreateRefreshToken(user.Id);

        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();

        return new AuthResponseDto(accessToken, refreshToken.Token, user.UserName!, user.Email!, expiration);
    }

    private static RefreshToken CreateRefreshToken(Guid userId)
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = Convert.ToBase64String(randomNumber),
            ExpiresAt = DateTime.UtcNow.AddDays(7) // Refresh token lives for 7 days
        };
    }
}