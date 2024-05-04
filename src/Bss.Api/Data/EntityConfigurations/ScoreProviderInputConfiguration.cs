using Bss.Api.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bss.Api.Data.EntityConfigurations
{
    public class ScoreProviderInputConfiguration : IEntityTypeConfiguration<ScoreProviderInput>
    {
        public void Configure(EntityTypeBuilder<ScoreProviderInput> builder)
        {
            builder.HasKey(spi => new { spi.ScoreProviderId, spi.InputId });

            builder
                .HasOne(spi => spi.ScoreProvider)
                .WithMany(sp => sp.ScoreProviderInputs)
                .HasForeignKey(spi => spi.ScoreProviderId);

            builder
                .HasOne(spi => spi.Input)
                .WithMany(i => i.ScoreProviderInputs)
                .HasForeignKey(spi => spi.InputId);
        }
    }
}
