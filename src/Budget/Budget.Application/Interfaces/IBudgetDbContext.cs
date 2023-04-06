using Budget.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Budget.Application.Interfaces
{
    public interface IBudgetDbContext
    {
        DbSet<Account> Accounts { get; set; }

        DbSet<Currency> Currencies { get; set; }

        DbSet<PaymentType> PaymentTypes { get; set; }

        DbSet<Record> Records { get; set; }

        DbSet<Category> Categories { get; set; }

        DbSet<UserCategory> UserCategories { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
