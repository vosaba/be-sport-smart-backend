using Bss.Infrastructure.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Bss.Component.Identity.Extensions;

internal static class ServiceCollectionExtensions
{
    private const string AddStoresMethodName = "AddStores";

    /// <summary>
    /// Adds the necessary Entity Framework stores for the specified user and role types to the IdentityBuilder.
    /// This method maintains the independence of the identity registration module from the DAL layer by only knowing
    /// the DbContext interface. As such, it uses reflection to call the registration of stores for the specified types.
    /// This ensures that the registration of the stores is correctly handled even though the exact implementation of the
    /// DbContext is abstracted away.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <typeparam name="TDbContext">The type representing the DbContext.</typeparam>
    /// <param name="identityBuilder">The IdentityBuilder to add the stores to.</param>
    /// <returns>The updated IdentityBuilder.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the method to add the stores cannot be found.</exception>
    public static IdentityBuilder AddEntityFrameworkStores<TUser, TRole, TDbContext>(this IdentityBuilder identityBuilder)
    {
        var methodInfo = typeof(IdentityEntityFrameworkBuilderExtensions)
            .GetMethod(AddStoresMethodName, BindingFlags.NonPublic | BindingFlags.Static);

        if (methodInfo == null)
        {
            throw new InvalidOperationException($"The method '{AddStoresMethodName}' could not be found.");
        }

        var dbContextType = GetDbContextImplementationType<TDbContext>();

        methodInfo.Invoke(null, [identityBuilder.Services, typeof(TUser), typeof(TRole), dbContextType]);

        return identityBuilder;
    }


    /// <summary>
    /// Retrieves the concrete implementation type for the specified DbContext interface type.
    /// This method scans all assemblies loaded in the current application domain to find a class
    /// that implements the specified DbContext interface type. If no implementation is found,
    /// an InvalidOperationException is thrown.
    /// </summary>
    /// <typeparam name="TDbContext">The type representing the DbContext interface.</typeparam>
    /// <returns>The concrete implementation type of the specified DbContext interface.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no implementation for the specified DbContext type is found.</exception>
    private static Type GetDbContextImplementationType<TDbContext>()
    {
        var dbContextType = AssemblyManager.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(TDbContext)))
            .FirstOrDefault();

        return dbContextType == null
            ? throw new InvalidOperationException($"The implementation for type '{typeof(TDbContext).Name}' could not be found.")
            : dbContextType;
    }
}
