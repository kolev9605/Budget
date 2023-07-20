using System;
using System.Collections.Generic;

namespace Budget.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        public (string token, DateTime validTo) GenerateToken(IEnumerable<string> userRoles, string userId);
    }
}
