using ItaTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaTask.Infrastructure.Persistence.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasQueryFilter(c => !c.IsDeleted);

        builder.Property(c => c.Name)
            .IsRequired();

        builder.HasData(
            new City { Id = 1, Name = "თბილისი", CreatedAt = DateTime.UtcNow },
            new City { Id = 2, Name = "ბათუმი", CreatedAt = DateTime.UtcNow },
            new City { Id = 3, Name = "ქუთაისი", CreatedAt = DateTime.UtcNow },
            new City { Id = 4, Name = "რუსთავი", CreatedAt = DateTime.UtcNow },
            new City { Id = 5, Name = "გორი", CreatedAt = DateTime.UtcNow }
        );
    }
}