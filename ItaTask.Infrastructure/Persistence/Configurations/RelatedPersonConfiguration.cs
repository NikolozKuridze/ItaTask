using ItaTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaTask.Infrastructure.Persistence.Configurations;

public class RelatedPersonConfiguration : IEntityTypeConfiguration<RelatedPerson>
{
    public void Configure(EntityTypeBuilder<RelatedPerson> builder)
    {
        builder.HasQueryFilter(p => !p.IsDeleted);

        builder.HasOne(r => r.RelatedTo)
            .WithMany(p => p.RelatedPersons)
            .HasForeignKey(r => r.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.RelatedTo)
            .WithMany()
            .HasForeignKey(r => r.RelatedPersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => new { r.PersonId, r.RelatedPersonId })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}