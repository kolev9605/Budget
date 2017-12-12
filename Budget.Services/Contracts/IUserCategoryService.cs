namespace Budget.Services.Contracts
{
    using System.Threading.Tasks;

    public interface IUserCategoryService
    {
        Task<bool> SaveInitialUserCategoriesAsync(string userId);
    }
}
