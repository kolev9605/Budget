namespace Budget.Api.Models.Authentication;

public record RegistrationRequest(
    string Username,
    string Password,
    string Email);
