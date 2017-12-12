namespace Budget.Services
{
    using Budget.Data;
    using Budget.Data.Models;
    using Budget.Services.Contracts;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserCategoryService : IUserCategoryService
    {
        private readonly BudgetDbContext context;

        public UserCategoryService(BudgetDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> SaveInitialUserCategoriesAsync(string userId)
        {
            var categories = this.context.Categories
                .Where(c => c.IsPrimary)
                .Select(c => new UserCategory
                {   
                    CategoryId = c.Id,
                    UserId = userId
                });

            var user = await this.context.Users.FindAsync(userId);
            user.UserCategories = new List<UserCategory>(categories);

            return await context.SaveChangesAsync() > 0;
        }
    }
}
