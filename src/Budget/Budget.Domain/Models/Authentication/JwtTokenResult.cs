namespace Budget.Domain.Models.Authentication;

public record JwtTokenResult(
    string Token,
    DateTime ValidTo
);
