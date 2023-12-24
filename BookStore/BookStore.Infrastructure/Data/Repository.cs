using System.Linq.Expressions;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using BookStore.Domain.Base;
using BookStore.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Data;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : BaseEntity
{
    private readonly BookStoreDbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(BookStoreDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }
    
    public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
    {
        return await SpecificationEvaluator.Default.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), specification)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return (await _dbSet.AddAsync(entity, cancellationToken)).Entity;
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public IQueryable<TEntity> Query(ISpecification<TEntity> specification, params Expression<Func<TEntity, object>>[] includes)
    {
        var dbSet = SpecificationEvaluator.Default.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), specification);
        var query = includes
            .Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>(dbSet, (current, include) => current.Include(include));

        return query ?? dbSet;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            if (_dbContext.Database.CurrentTransaction != null)
            {
                await _dbContext.Database.CurrentTransaction.RollbackAsync(cancellationToken);
            }

            throw;
        }
    }

    public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity> specification, CancellationToken ct)
    {
        var item = await SpecificationEvaluator.Default.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), specification)
            .AsNoTracking()
            .FirstOrDefaultAsync(ct);

        return item;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return await Task.Run(() => _dbSet.Update(entity).Entity, cancellationToken);
    }

    public async Task<TEntity> RemoveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return await Task.Run(() => _dbSet.Remove(entity).Entity, cancellationToken);
    }
}