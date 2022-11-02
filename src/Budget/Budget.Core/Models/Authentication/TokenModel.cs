﻿using System;
using System.Collections.Generic;

namespace Budget.Core.Models.Authentication
{
    public class TokenModel
    {
        public string Token { get; set; }

        public DateTime ValidTo { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
