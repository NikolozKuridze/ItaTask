using ItaTask.Domain.Entities;
using ItaTask.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ItaTask.Infrastructure.Persistence.Repositories;

public class CityRepository(ItaTaskDbContext context) : RepositoryBase<City>(context), ICityRepository
{
    public async Task<List<City>> GetAllCitiesAsync(CancellationToken cancellationToken)
    {
        return await Context.Cities
            .Where(x => !x.IsDeleted)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}