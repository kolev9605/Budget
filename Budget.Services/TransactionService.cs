namespace Budget.Services
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Budget.Data;
    using Budget.Data.Models;
    using Budget.Data.Models.Enums;
    using Budget.Services.Contracts;
    using Budget.Services.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TransactionService : ITransactionService
    {
        private readonly BudgetDbContext context;
        private readonly IMapper mapper;

        public TransactionService(
            BudgetDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TransactionServiceModel>> GetAllByUserIdAsync(string userId)
        {
            var transactions = await this.context
                .Transactions
                .Where(t => t.UserId == userId)
                .ProjectTo<TransactionServiceModel>()
                .ToListAsync();

            return transactions;
        }

        public async Task<bool> AddTransactionAsync(decimal amount, string userId, int categoryId, string description, TransactionType type)
        {
            var user = this.context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new InvalidOperationException($"There is not existing user with id: {userId}.");
            }

            var transaction = new Transaction
            {
                Amount = amount,
                UserId = userId,
                CategoryId = categoryId,
                Date = DateTime.UtcNow,
                Description = description
            };

            user.Balance = type == TransactionType.Income ? user.Balance += amount : user.Balance -= amount;
            var result = await this.context.Transactions.AddAsync(transaction);

            return await this.context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteTransactionAsync(int transactionId)
        {
            var transaction = await this.context.Transactions
                .Where(t => t.Id == transactionId)
                .Select(t => new Transaction
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    UserId = t.UserId,
                    Date = t.Date,
                    Category = t.Category
                })
                .FirstOrDefaultAsync();

            if(transaction == null)
            {
                return false;
            }

            var transactionAmount = transaction.Amount;
            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Id == transaction.UserId);
            if (user == null)
            {
                throw new InvalidOperationException($"There is not existing user with id: {user.Id}.");
            }

            user.Balance = transaction.Category.TransactionType == TransactionType.Income ? user.Balance -= transactionAmount: user.Balance += transactionAmount;
            this.context.Transactions.Remove(transaction);
            return await this.context.SaveChangesAsync() > 0;
        }
    }
}
