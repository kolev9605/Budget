using ErrorOr;

namespace Budget.Domain.Common.Errors;

public static partial class Errors
{
    public static class Category
    {
        public static Error CategoryNotFound => Error.NotFound(
            code: "Category.NotFound",
            description: "Missing category."
        );
    }
}
