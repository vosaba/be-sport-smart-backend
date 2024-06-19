﻿using Bss.Component.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bss.Dal.EntityConfigurations.Core;

public class MeasureConfiguration : IEntityTypeConfiguration<Measure>
{
    public void Configure(EntityTypeBuilder<Measure> builder)
    {
        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasIndex(x => x.Type);

        builder.Property(i => i.Options)
            .HasColumnType("text[]");
    }
}
