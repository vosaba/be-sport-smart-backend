using Bss.Api.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Bss.Api.Data
{
    public abstract class DbContextWithEntityConfiguration : IdentityDbContext<AppUser>
    {
        protected DbContextWithEntityConfiguration(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            AddEntityConfigurations(modelBuilder);
        }

        private void AddEntityConfigurations(ModelBuilder modelBuilder)
        {
            static bool IsEntityTypeConfigInterface(Type type) => type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>);

            var configurationTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.GetInterfaces().FirstOrDefault(IsEntityTypeConfigInterface) != null && !x.IsAbstract);

            // Method ApplyConfiguration has 2 overloads. That is why need to search by param type.
            var applyConfiguration = modelBuilder
                .GetType()
                .GetMethods()
                .FirstOrDefault(x => x.Name == nameof(modelBuilder.ApplyConfiguration)
                                     && x.GetParameters().FirstOrDefault(p => IsEntityTypeConfigInterface(p.ParameterType)) != null);

            if (applyConfiguration == null)
            {
                throw new ApplicationException($"Could not find method {nameof(modelBuilder.ApplyConfiguration)} in {modelBuilder.GetType()}", null);
            }

            foreach (var configurationType in configurationTypes)
            {
                var entityType = configurationType
                    .GetInterfaces()
                    .FirstOrDefault(IsEntityTypeConfigInterface)
                    ?.GenericTypeArguments
                    .First();

                if (entityType == null)
                {
                    throw new ApplicationException($"Could not get entity type for configuration type {configurationType.FullName}", null);
                }

                var genericApplyConfiguration = applyConfiguration.MakeGenericMethod(entityType);
                genericApplyConfiguration.Invoke(modelBuilder, new[] { Activator.CreateInstance(configurationType) });
            }
        }
    }
}