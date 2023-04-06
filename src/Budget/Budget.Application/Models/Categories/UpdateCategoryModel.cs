using Budget.Domain.Entities;

namespace Budget.Application.Models.Categories
{
    public class UpdateCategoryModel : BaseCrudCategoryModel
    {
        public int Id { get; set; }
    }
}
