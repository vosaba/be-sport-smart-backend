using Bss.Api.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bss.Api.Data.EntityConfigurations
{
    public class InputConfiguration : IEntityTypeConfiguration<Input>
    {
        public void Configure(EntityTypeBuilder<Input> builder)
        {
            builder.Property(i => i.Options)
                .HasColumnType("text[]");
        }
    }
}
