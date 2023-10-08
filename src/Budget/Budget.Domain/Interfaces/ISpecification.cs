using Budget.Domain.Entities.Base;
using Budget.Domain.Models.Specifications;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Budget.Domain.Interfaces;

public interface ISpecification<TEntity>
    where TEntity : BaseEntity
{
    public ICollection<Expression<Func<TEntity, bool>>> CriteriaExpressions { get; }

    public List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> Includes { get; }

    public ICollection<SortDescriptor> SortDescriptors { get; }

    public Expression<Func<TEntity, TEntity>> MappingExpression { get; }
}
