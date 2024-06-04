using ErrorOr;

namespace Budget.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error UserNotFound => Error.NotFound(
            code: "User.NotFound",
            description: "User not found."
        );

        public static Error UserAlreadyExists => Error.Conflict(
            code: "User.UserAlreadyExists",
            description: "User already exists."
        );

        public static Error AuthenticationFailed  => Error.Unauthorized(
            code: "User.AuthenticationFailed",
            description: "Authentication failed."
        );
    }
}
