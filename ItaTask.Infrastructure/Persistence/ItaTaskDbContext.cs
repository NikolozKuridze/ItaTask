using ItaTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ItaTask.Infrastructure.Persistence;

public class ItaTaskDbContext(DbContextOptions<ItaTaskDbContext> options) : DbContext(options)
{
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<PhoneNumber> PhoneNumbers => Set<PhoneNumber>();
    public DbSet<RelatedPerson> RelatedPersons => Set<RelatedPerson>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ItaTaskDbContext).Assembly);
    }
}