using Budget.Core.Entities;

namespace Budget.Core.Interfaces
{
    public interface IBudgetDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
