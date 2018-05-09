namespace Budget.Services.Contracts
{
    using Budget.Data.Models.Enums;
    using Budget.Services.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICategoryService
    {
        Task<IEnumerable<CategoryInfoServiceModel>> GetAllUserCategoriesByTypeAsync(string userId, TransactionType transactionType);

        Task<TransactionType> GetTransactionTypeByCategoryIdAsync(int categoryId);

        Task<int> AddOrGetCategoryAsync(string name, TransactionType type, string rgbColor, string userId = null);

        Task<bool> DeleteUserCategoryAsync(int categoryId, string userId);

        Task<IEnumerable<CategoryInfoServiceModel>> GetAllCategoriesInfo();

        IEnumerable<string> GetAllCategoryColors();
    }
}
