using Budget.Domain.Entities.Base;
using Budget.Domain.Interfaces;
using Budget.Domain.Models.Specifications;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Budget.Application.Specifications
{
    public abstract class Specification<TEntity> : ISpecification<TEntity>
        where TEntity : BaseEntity
    {
        public ICollection<Expression<Func<TEntity, bool>>> CriteriaExpressions { get; private set; }
            = new List<Expression<Func<TEntity, bool>>>();

        public List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> Includes { get; }
            = new List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>();

        public ICollection<SortDescriptor> SortDescriptors { get; private set; }

        public Expression<Func<TEntity, TEntity>> MappingExpression { get; private set; }

        protected void AddCriteria(Expression<Func<TEntity, bool>> criteria)
            => CriteriaExpressions.Add(criteria);

        protected void AddInclude(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression)
            => Includes.Add(includeExpression);

        protected void AddSortDescriptor(SortDescriptor sortDescriptor)
            => SortDescriptors.Add(sortDescriptor);

        protected void SetMappingExpression(Expression<Func<TEntity, TEntity>> mappingExpression)
            => MappingExpression = mappingExpression;
    }


}
