using Snipcode.Domain.Entities;

namespace Snipcode.Application.Interfaces;

public interface IJwtTokenGenerator
{
    (string Token, DateTime Expiration) GenerateToken(ApplicationUser user);
}