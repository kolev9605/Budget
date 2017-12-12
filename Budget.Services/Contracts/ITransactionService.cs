namespace Budget.Services.Contracts
{
    using Budget.Data.Models.Enums;
    using Budget.Services.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITransactionService
    {
        Task<IEnumerable<TransactionServiceModel>> GetAllByUserIdAndTypeAsync(string userId, TransactionType type);

        Task<bool> AddTransactionAsync(decimal amount, string userId, int categoryId, string description);
    }
}
