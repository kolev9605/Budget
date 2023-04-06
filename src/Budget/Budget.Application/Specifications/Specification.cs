using Budget.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Budget.Application.Specifications
{
    public abstract class Specification<TEntity>
        where TEntity : BaseEntity
    {
        public Expression<Func<TEntity, bool>> CriteriaExpression { get; private set; }

        public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; private set; }
            = new List<Expression<Func<TEntity, object>>>();

        public Expression<Func<TEntity, object>> OrderByExpression { get; private set; }

        public Expression<Func<TEntity, object>> OrderByDescendingExpression { get; private set; }

        public Expression<Func<TEntity, TEntity>> MappingExpression { get; private set; }

        protected void SetCriteria(Expression<Func<TEntity, bool>> criteria)
            => CriteriaExpression = criteria;

        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
            => IncludeExpressions.Add(includeExpression);

        protected void SetOrderBy(Expression<Func<TEntity, object>> orderByExpression)
            => OrderByDescendingExpression = orderByExpression;

        protected void SetOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
            => OrderByDescendingExpression = orderByDescendingExpression;

        protected void SetMappingExpression(Expression<Func<TEntity, TEntity>> mappingExpression)
            => MappingExpression = mappingExpression;
    }


}
