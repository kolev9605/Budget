namespace Budget.Services.Contracts
{
    using Budget.Data.Models.Enums;
    using Budget.Services.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICategoryService
    { 
        Task<IEnumerable<CategoryServiceModel>> GetAllPrimaryAsync();

        Task<IEnumerable<CategoryServiceModel>> GetAllUserCategoriesByTypeAsync(string userId, TransactionType transactionType);

        Task<TransactionType> GetTransactionTypeByCategoryIdAsync(int categoryId);
    }
}
