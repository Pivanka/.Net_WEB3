﻿using System.Linq.Expressions;
using Ardalis.Specification;
using BookStore.Domain.Base;

namespace BookStore.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken);
    IQueryable<TEntity> Query(ISpecification<TEntity> specification, params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task<TEntity> RemoveAsync(TEntity entity, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<TEntity?> GetByIdAsync(ISpecification<TEntity> specification, CancellationToken ct);
}