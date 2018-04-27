namespace Budget.Services.Contracts
{
    using Budget.Data.Models.Enums;
    using Budget.Services.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICategoryService
    {
        Task<IEnumerable<UserCategoryServiceModel>> GetAllUserCategoriesByTypeAsync(string userId, TransactionType transactionType);

        Task<TransactionType> GetTransactionTypeByCategoryIdAsync(int categoryId);

        Task<int> AddOrGetCategoryAsync(string name, TransactionType type, string rgbColor);

        Task<bool> AddUserCategoryAsync(int categoryId, string userId);

        Task<bool> DeleteUserCategoryAsync(int categoryId, string userId);

        IEnumerable<string> GetAllCategoryColors();

        Task<IEnumerable<CategoryInfoServiceModel>> GetAllCategoriesInfo();
    }
}
