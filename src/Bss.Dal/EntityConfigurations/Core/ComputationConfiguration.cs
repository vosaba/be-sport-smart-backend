using Bss.Core.Bl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bss.Dal.EntityConfigurations.Core;

public class ComputationConfiguration : IEntityTypeConfiguration<Computation>
{
    public void Configure(EntityTypeBuilder<Computation> builder)
    {
        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasIndex(x => x.Type);
        builder.HasIndex(x => x.Engine);

        builder.Property("_requiredComputations")
            .HasColumnName("RequiredComputations")
            .HasColumnType("text[]");

        builder.Property("_requiredMeasures")
            .HasColumnName("RequiredMeasures")
            .HasColumnType("text[]");

        builder.Property(x => x.Availability)
            .HasConversion<int>()
            .IsRequired();
    }
}
