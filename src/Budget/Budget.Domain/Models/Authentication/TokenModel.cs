using System;
using System.Collections.Generic;

namespace Budget.Domain.Models.Authentication;

public class TokenModel
{
    public string Token { get; set; } = null!;

    public DateTime ValidTo { get; set; }

    public IEnumerable<string> Roles { get; set; } = new List<string>();
}
