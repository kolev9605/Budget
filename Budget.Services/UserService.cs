namespace Budget.Services
{
    using System.Threading.Tasks;
    using Budget.Services.Contracts;
    using Budget.Data;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using System;

    public class UserService : IUserService
    {
        private readonly BudgetDbContext context;
        private readonly IMapper mapper;

        public UserService(
            BudgetDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<decimal?> GetUserBalanceAsync(string userId)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new InvalidOperationException($"There is not existing user with id: {userId}.");
            }

            return user.Balance;
        }
    }
}
