namespace Budget.Api.Models.Authentication;

public record LoginRequest(
    string Email,
    string Password);
