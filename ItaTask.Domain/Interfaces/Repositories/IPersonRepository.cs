using ItaTask.Domain.Entities;
using ItaTask.Domain.Enums;

namespace ItaTask.Domain.Interfaces.Repositories;

public interface IPersonRepository : IRepositoryBase<Person>
{
    Task<Person?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<Person?> GetByPersonalNumberAsync(string personalNumber, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Person> Items, int Total)> GetPagedAsync(string? searchTerm, int page, int pageSize,
        CancellationToken cancellationToken = default);

    Task<IDictionary<RelationType, int>> GetRelatedPersonsCountByTypeAsync(int personId,
        CancellationToken cancellationToken = default);
}