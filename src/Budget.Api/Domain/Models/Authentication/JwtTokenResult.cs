namespace Budget.Api.Domain.Models.Authentication;

public record JwtTokenResult(
    string Token,
    DateTime ValidTo
);
