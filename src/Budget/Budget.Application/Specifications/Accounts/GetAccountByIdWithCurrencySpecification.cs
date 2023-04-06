using Budget.Domain.Entities;

namespace Budget.Application.Specifications.Accounts
{
    public class GetAccountByIdWithCurrencySpecification : Specification<Account>
    {
        public GetAccountByIdWithCurrencySpecification(int accountId, string userId)
        {
            AddInclude(a => a.Currency);
            AddInclude(a => a.Records);

            SetCriteria(a => a.Id == accountId && a.UserId == userId);
        }
    }
}
