using Budget.Domain.Entities;
using Budget.Domain.Models.Categories;
using Mapster;

namespace Budget.Application.Mappings;

public class CategoryMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Category, CategoryModel>().MaxDepth(2);

        config.NewConfig<(CreateCategoryModel Category, string UserId), Category>()
          .Map(dest => dest, src => src.Category)
          .AfterMapping((src, dest) =>
          {
              dest.Users.Add(new UserCategory()
              {
                  UserId = src.UserId
              });
          });
    }
}
