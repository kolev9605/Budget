using Budget.Core.Entities.Base;
using System.Collections.Generic;

namespace Budget.Core.Entities
{
    public class Category : IBaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Record> Records { get; set; } = new List<Record>();

        public CategoryType CategoryType { get; set; }

        public int? ParentCategoryId { get; set; }

        public Category ParentCategory { get; set; }

        public ICollection<Category> SubCategories { get; set; } = new List<Category>();

        public ICollection<UserCategory> Users { get; set; } = new List<UserCategory>();

        public bool IsInitial { get; set; } = true;

    }
}
