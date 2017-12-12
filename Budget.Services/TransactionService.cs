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

        public async Task<IEnumerable<TransactionServiceModel>> GetAllByUserIdAndTypeAsync(string userId, TransactionType type)
        {
            var transactions = await this.context
                .Transactions
                .Where(t => t.UserId == userId && t.Category.TransactionType == type)
                .ProjectTo<TransactionServiceModel>()
                .ToListAsync();

            return transactions;
        }

        public async Task<bool> AddTransactionAsync(decimal amount, string userId, int categoryId, string description)
        {
            var transaction = new Transaction
            {
                Amount = amount,
                UserId = userId,
                CategoryId = categoryId,
                Date = DateTime.UtcNow,
                Description = description
            };

            var result = await this.context.Transactions.AddAsync(transaction);
            return await this.context.SaveChangesAsync() > 0;
        }
    }
}
