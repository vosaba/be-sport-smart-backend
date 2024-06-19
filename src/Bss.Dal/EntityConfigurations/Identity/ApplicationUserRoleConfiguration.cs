using Bss.Component.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bss.Dal.EntityConfigurations.Identity;

public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
{
    private static Guid AdminRoleId => Guid.Parse("b1ff449a-4a12-449b-aef4-64f6c12e6fa2");
    private static Guid TrainerRoleId => Guid.Parse("9d58223d-94ab-4777-970f-e1dbf7e20c5b");
    private static Guid UserRoleId => Guid.Parse("3e2da8f2-3298-4969-89d8-632adb81c18d");

    public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
    {
        var roles = new List<ApplicationUserRole>
        {
            new() {
                Id = AdminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new() {
                Id = TrainerRoleId,
                Name = "Trainer",
                NormalizedName = "TRAINER"
            },
            new() {
                Id = UserRoleId,
                Name = "User",
                NormalizedName = "USER"
            },
        };

        builder.HasData(roles);
    }
}