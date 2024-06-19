using Bss.Component.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bss.Dal.EntityConfigurations.Core;

public class ComputationConfiguration : IEntityTypeConfiguration<Computation>
{
    public void Configure(EntityTypeBuilder<Computation> builder)
    {
        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasIndex(x => x.Type);

        builder.Property("_requiredComputations")
            .HasColumnName("RequiredComputations")
            .HasColumnType("text[]");

        builder.Property("_requiredMeasures")
            .HasColumnName("RequiredMeasures")
            .HasColumnType("text[]");
    }
}
