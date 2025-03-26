namespace Budget.Api.Models.Authentication;

public record AuthenticationResponse(
    string Token,
    DateTime ValidTo
);
