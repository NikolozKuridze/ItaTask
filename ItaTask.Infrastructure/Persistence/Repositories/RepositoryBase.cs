using ItaTask.Domain.Entities;
using ItaTask.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ItaTask.Infrastructure.Persistence.Repositories;

public abstract class RepositoryBase<T>(ItaTaskDbContext context) : IRepositoryBase<T>
    where T : EntityBase
{
    protected readonly ItaTaskDbContext Context = context;
    private readonly DbSet<T> DbSet = context.Set<T>();

    public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(x => !x.IsDeleted)
            .AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.UpdateAt = DateTime.UtcNow;
        Context.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdateAt = DateTime.UtcNow;
        await UpdateAsync(entity, cancellationToken);
    }
}