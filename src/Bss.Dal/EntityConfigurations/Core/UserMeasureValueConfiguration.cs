using Bss.Component.Core.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bss.Dal.EntityConfigurations.Core;

public class UserMeasureValueConfiguration : IEntityTypeConfiguration<UserMeasureValue>
{
    public void Configure(EntityTypeBuilder<UserMeasureValue> builder)
    {
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.Name);
    }
}