using Budget.Domain.Entities;
using Mapster;

namespace Budget.Domain.Models.Categories;

public record CategoryModel(
    Guid Id,
    string Name,
    CategoryType CategoryType,
    Guid? ParentCategoryId,
    bool IsInitial,
    // TODO: Separate model for the categories
    // IEnumerable<SubCategoryModel> SubCategories,
    DateTimeOffset CreatedOn,
    DateTimeOffset UpdatedOn);

public record SubCategoryModel(
    Guid Id,
    string Name,
    CategoryType CategoryType,
    Guid? ParentCategoryId,
    bool IsInitial,
    DateTimeOffset CreatedOn,
    DateTimeOffset UpdatedOn
);

public class CategoryModelMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // config.NewConfig<Category, CategoryModel>()
        //     .Map(dest => dest.SubCategories, src => src.SubCategories);

    }
}
