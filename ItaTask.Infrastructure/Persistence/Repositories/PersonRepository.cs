using ItaTask.Domain.Entities;
using ItaTask.Domain.Enums;
using ItaTask.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ItaTask.Infrastructure.Persistence.Repositories;

public class PersonRepository(ItaTaskDbContext context) : RepositoryBase<Person>(context), IPersonRepository
{
    public async Task<Person?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Persons
            .Include(x => x.City)
            .Include(x => x.PhoneNumbers.Where(p => !p.IsDeleted))
            .Include(x => x.RelatedPersons.Where(r => !r.IsDeleted))
            .ThenInclude(x => x.RelatedTo)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
    }

    public async Task<Person?> GetByPersonalNumberAsync(string personalNumber,
        CancellationToken cancellationToken = default)
    {
        return await Context.Persons
            .FirstOrDefaultAsync(x => x.PersonalNumber == personalNumber && !x.IsDeleted, cancellationToken);
    }

    public async Task<(IReadOnlyList<Person> Items, int Total)> GetPagedAsync(
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = Context.Persons
            .Include(x => x.City)
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(x =>
                EF.Functions.Like(x.FirstName.ToLower(), $"%{searchTerm}%") ||
                EF.Functions.Like(x.LastName.ToLower(), $"%{searchTerm}%") ||
                EF.Functions.Like(x.PersonalNumber, $"%{searchTerm}%"));
        }

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (items, total);
    }

    public async Task<IDictionary<RelationType, int>> GetRelatedPersonsCountByTypeAsync(
        int personId,
        CancellationToken cancellationToken = default)
    {
        return await Context.RelatedPersons
            .Where(x => x.PersonId == personId && !x.IsDeleted)
            .GroupBy(x => x.RelationType)
            .ToDictionaryAsync(
                g => g.Key,
                g => g.Count(),
                cancellationToken);
    }
}