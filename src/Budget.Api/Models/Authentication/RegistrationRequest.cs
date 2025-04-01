namespace Budget.Api.Models.Authentication;

public record RegistrationRequest(
    string Password,
    string Email);
