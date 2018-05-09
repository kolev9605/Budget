using System.Threading.Tasks;

namespace Budget.Services.Contracts
{
    public interface IUserService
    {
        Task<decimal?> GetUserBalanceAsync(string userId);
    }
}
