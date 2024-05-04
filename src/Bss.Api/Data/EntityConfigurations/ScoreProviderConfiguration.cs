using Bss.Api.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bss.Api.Data.EntityConfigurations
{
    public class ScoreProviderConfiguration : IEntityTypeConfiguration<ScoreProvider>
    {
        public void Configure(EntityTypeBuilder<ScoreProvider> builder)
        {
            builder.Property(sp => sp.DependentProviders)
                .HasColumnType("text[]");
        }
    }
}
