using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>, IApplicationDBContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {

        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        public DbSet<Input> Inputs { get; set; }
        public DbSet<ScoreProvider> ScoreProviders { get; set; }
        public DbSet<ScoreProviderInput> ScoreProviderInputs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));

            builder.Entity<Portfolio>()
                .ToTable("Portfolios")
                .HasOne(u => u.AppUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.AppUserId);

            builder.Entity<Portfolio>()
                .HasOne(u => u.Stock)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.StockId);

            builder.Entity<Comment>()
                .ToTable("Comments");

            builder.Entity<Stock>()
                .ToTable("Stocks")
                .HasKey(s => s.Id);

            builder.Entity<Stock>()
                .Property(s => s.Purchase)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Stock>()
                .Property(s => s.LastDiv)
                .HasColumnType("decimal(18,2)");

            builder.Entity<ScoreProviderInput>()
                .HasKey(spi => new { spi.ScoreProviderId, spi.InputId });

            builder.Entity<ScoreProviderInput>()
                .HasOne(spi => spi.ScoreProvider)
                .WithMany(sp => sp.ScoreProviderInputs)
                .HasForeignKey(spi => spi.ScoreProviderId);

            builder.Entity<ScoreProviderInput>()
                .HasOne(spi => spi.Input)
                .WithMany(i => i.ScoreProviderInputs)
                .HasForeignKey(spi => spi.InputId);

            builder.Entity<Input>()
                .Property(i => i.Options)
                .HasColumnType("text[]");

            // Configure array of strings for PostgreSQL
            builder.Entity<ScoreProvider>()
                .Property(sp => sp.DependentProviders)
                .HasColumnType("text[]");

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "Trainer",
                    NormalizedName = "TRAINER"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}