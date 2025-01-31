using ItaTask.Domain.Interfaces;

namespace ItaTask.Infrastructure.Persistence;

public class UnitOfWork(ItaTaskDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}