using System;
using System.Collections.Generic;

namespace Budget.Domain.Interfaces;

public interface IJwtTokenGenerator
{
    public (string token, DateTime validTo) GenerateToken(IEnumerable<string> userRoles, string userId);
}
