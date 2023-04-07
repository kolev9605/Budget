using Budget.Domain.Entities.Base;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Budget.Application.Specifications
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> ApplySpecification<TEntity>(
            this IQueryable<TEntity> query,
            Specification<TEntity> specification)
            where TEntity : BaseEntity
        {
            if (specification.IncludeExpressions is not null && specification.IncludeExpressions.Any())
            {
                query = specification.IncludeExpressions.Aggregate(
                    query,
                    (current, includeExpression) => current.Include(includeExpression));
            }

            if (specification.CriteriaExpression is not null)
            {
                query = query.Where(specification.CriteriaExpression);                
            }

            if (specification.OrderByExpression is not null)
            {
                query = query.OrderBy(specification.OrderByExpression);
            }

            if (specification.OrderByDescendingExpression is not null )
            {
                query = query.OrderByDescending(specification.OrderByDescendingExpression);
            }

            return query;
        }
    }
}
