﻿using System.ComponentModel.DataAnnotations;

namespace Budget.Core.Models.Authentication
{
    public class LoginModel
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
