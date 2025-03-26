using Budget.Domain.Models.Authentication;

namespace Budget.Domain.Interfaces;

public interface IJwtTokenGenerator
{
    public JwtTokenResult GenerateToken(IEnumerable<string> userRoles, string userId, string email);
}
