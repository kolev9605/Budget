using Budget.Domain.Entities;

namespace Budget.Domain.Models.Categories
{
    public class UpdateCategoryModel : BaseCrudCategoryModel
    {
        public int Id { get; set; }
    }
}
