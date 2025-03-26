namespace Budget.Api.Models.Authentication;

public record LoginRequest(
    string Username,
    string Password);
