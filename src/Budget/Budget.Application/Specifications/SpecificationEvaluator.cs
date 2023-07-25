using Budget.Domain.Entities.Base;
using Budget.Domain.Interfaces;
using Budget.Domain.Models.Specifications;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Budget.Application.Specifications
{
    public static class SpecificationEvaluator
    {
        public async static Task<IEnumerable<TResult>> GetListAsync<TEntity, TResult>(
            this IQueryable<TEntity> query,
            ISpecification<TEntity> specification)
            where TEntity : BaseEntity
        {
            query = ApplySpecification(query, specification);

            return await query.ProjectToType<TResult>().ToListAsync();
        }

        public async static Task<TResult> GetOneAsync<TEntity, TResult>(
            this IQueryable<TEntity> query,
            ISpecification<TEntity> specification)
            where TEntity : BaseEntity
        {
            query = ApplySpecification(query, specification);

            return await query.ProjectToType<TResult>().FirstOrDefaultAsync();
        }

        private static IQueryable<TEntity> ApplySpecification<TEntity>(
            this IQueryable<TEntity> query,
            ISpecification<TEntity> specification)
            where TEntity : BaseEntity
        {
            if (specification.Includes is not null && specification.Includes.Any())
            {
                query = specification.Includes.Aggregate(
                    query,
                    (current, include) => include(current));
            }

            if (specification.CriteriaExpressions is not null && specification.CriteriaExpressions.Any())
            {
                foreach (var filterExpr in specification.CriteriaExpressions)
                {
                    query = query.Where(filterExpr);
                }
            }

            if (specification.SortDescriptors is not null && specification.SortDescriptors.Any())
            {
                query = ApplySortings(query, specification.SortDescriptors);
            }

            return query;
        }

        private static IQueryable<TEntity> ApplySortings<TEntity>(IQueryable<TEntity> query, IEnumerable<SortDescriptor> sortings)
        {
            var index = 0;

            if (sortings != null && sortings.Any())
            {
                foreach (var sort in sortings)
                {
                    query = OrderBy(query, sort.Property, sort.Direction, thenBy: index != 0);
                    index++;
                }
            }

            return query;
        }

        private static IQueryable<TEntity> OrderBy<TEntity>(IQueryable<TEntity> source, string sortProperty, SortDirection sortDirection, bool thenBy = false)
        {
            var type = typeof(TEntity);

            var parameter = Expression.Parameter(type, "p");
            var parts = sortProperty.Split('.');

            var propertyAccess = parts.Aggregate<string, Expression>(parameter, Expression.Property) as MemberExpression;
            if (propertyAccess == null)
                throw new ArgumentException($"Invalid sort property: {sortProperty}.");

            var propertyInfo = propertyAccess.Member as PropertyInfo;
            if (propertyInfo == null)
                throw new ArgumentException($"Invalid sort property: {sortProperty}.");

            var orderByExp = Expression.Lambda(propertyAccess, parameter);

            string orderFunctionName;

            if (sortDirection == SortDirection.Ascending)
            {
                orderFunctionName = thenBy ? nameof(Queryable.ThenBy) : nameof(Queryable.OrderBy);
            }
            else
            {
                orderFunctionName = thenBy ? nameof(Queryable.ThenByDescending) : nameof(Queryable.OrderByDescending);
            }

            var resultExp = Expression.Call(typeof(Queryable), orderFunctionName, new Type[] { type, propertyInfo.PropertyType }, source.Expression, Expression.Quote(orderByExp));

            return source.Provider.CreateQuery<TEntity>(resultExp);
        }
    }
}
