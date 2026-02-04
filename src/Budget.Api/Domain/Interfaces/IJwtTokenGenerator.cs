using Budget.Api.Domain.Models.Authentication;

namespace Budget.Api.Domain.Interfaces;

public interface IJwtTokenGenerator
{
    public JwtTokenResult GenerateToken(IEnumerable<string> userRoles, string userId, string email);
}
