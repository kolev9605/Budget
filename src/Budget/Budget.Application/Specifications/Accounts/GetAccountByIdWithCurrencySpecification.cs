using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budget.Application.Specifications.Accounts
{
    public class GetAccountByIdWithCurrencySpecification : Specification<Account>
    {
        public GetAccountByIdWithCurrencySpecification(int accountId, string userId)
        {
            AddInclude(q => q
                .Include(a => a.Currency)
                .Include(a => a.Records)
                    .ThenInclude(r => r.PaymentType));

            AddCriteria(a => a.Id == accountId);
            AddCriteria(a => a.UserId == userId);
        }
    }
}
