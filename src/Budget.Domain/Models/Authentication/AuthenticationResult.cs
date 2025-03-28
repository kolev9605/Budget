namespace Budget.Domain.Models.Authentication;

public record AuthenticationResult(
    string Token,
    DateTime ValidTo
);
