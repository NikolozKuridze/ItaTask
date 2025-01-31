using ItaTask.Domain.Entities;

namespace ItaTask.Domain.Interfaces.Repositories;

public interface ICityRepository : IRepositoryBase<City>
{
   public Task<List<City>> GetAllCitiesAsync(CancellationToken cancellationToken); 

}