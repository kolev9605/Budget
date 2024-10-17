using ErrorOr;

namespace Budget.Domain.Common.Errors;

public static partial class Errors
{
    public static class Category
    {
        public static Error NotFound => Error.NotFound(
            code: "Category.NotFound",
            description: "Category doesn't exist."
        );

        public static Error AlreadyExists => Error.Conflict(
            code: "Category.AlreadyExists",
            description: "The category already exists."
        );

        public static Error AlreadyExistsForUser => Error.Conflict(
            code: "Category.AlreadyExistsForUser",
            description: "The category already exists for the given user."
        );

        public static Error HasRecords => Error.Validation(
            code: "Category.HasRecords",
            description: "Category has records and cannot be deleted."
        );

        public static Error HasSubCategories => Error.Validation(
            code: "Category.HasSubCategories",
            description: "Category has sub-categories linked to it and cannot be deleted."
        );

        public static Error CannotBecomeSubcategory => Error.Validation(
            code: "Category.CannotBecomeSubcategory",
            description: "Category has sub-categories linked to it and cannot be made sub-category itself."
        );
    }
}
