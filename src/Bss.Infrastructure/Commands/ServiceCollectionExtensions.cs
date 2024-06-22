using Bss.Infrastructure.Commands.Abstractions;
using Bss.Infrastructure.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Bss.Infrastructure.Commands;

public static partial class ServiceCollectionExtensions
{
    private const string DefaultCommandHandlerMethodNotFound = nameof(DefaultCommandHandlerMethodNotFound);

    public static void AddCommand(
        this IServiceCollection services,
        CommandDescriptor descriptor)
    {
        ValidateDescriptor(descriptor);

        var existingDescriptor = services
            .FirstOrDefault(s => s.ImplementationInstance is CommandDescriptor d &&
                d.CommandType == descriptor.CommandType &&
                d.MethodInfo == descriptor.MethodInfo);

        if (existingDescriptor != null)
        {
            services.Remove(existingDescriptor);
        }

        services.AddSingleton(descriptor);
    }

    public static void AddCommand(
        this IServiceCollection services,
        Type commandType,
        Func<Type, CommandDescriptor, bool>? configure = null)
    {
        var defaultNotValidMethod = typeof(ServiceCollectionExtensions)
            .GetMethod(DefaultCommandHandlerMethodNotFound, BindingFlags.Static | BindingFlags.NonPublic)!;

        var methodInfo = commandType
            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .Where(x => ValidHandler(x) && x.Name == CommandConstants.CommandMethodName)
            .Select(x => x)
            .FirstOrDefault() ?? defaultNotValidMethod;

        var descriptor = new CommandDescriptor(
            commandType,
            methodInfo,
            (commandType.FullName ?? commandType.Name).Split('.').First().ToLowerInvariant(),
            HttpMethod.Post,
            commandType.DefaultRoute());

        if (configure?.Invoke(commandType, descriptor) == false)
        {
            return;
        }

        services.AddCommand(descriptor);
    }

    public static void AddCommandsFromAssembly<T>(
        this IServiceCollection services,
        Action<Type, CommandDescriptor> configure)
    {
        AddCommandsFromAssembly<T>(
            services,
            (t, d) =>
            {
                configure(t, d);
                return true;
            });
    }

    public static void AddCommandsFromAssembly<T>(
        this IServiceCollection services,
        Func<Type, CommandDescriptor, bool> configure)
    {
        bool ConfigurationHandler(Type type, CommandDescriptor descriptor)
        {
            if (type.Name.EndsWith(CommandConstants.CommandTypeSuffix) && ValidHandler(type.GetMethod(CommandConstants.CommandMethodName)))
            {
                return configure(type, descriptor);
            }

            return false;
        }

        var commandTypes = typeof(T).Assembly
            .ExportedTypes
            .Where(t => t.IsPublic &&
                        t.IsClass &&
                        !t.IsAbstract &&
                        !t.IsGenericType &&
                        !t.IsInterface &&
                        !t.IsNested);

        foreach (var type in commandTypes)
        {
            services.AddCommand(type, ConfigurationHandler);
        }
    }

    public static void AddCommands<T>(this IServiceCollection services, string module, string? subEntity = null, IEnumerable<Predicate<Type>>? filters = null)
    {
        services.AddCommandsFromAssembly<T>((type, builder) =>
        {
            if (filters != null && filters.Any(x => !x(type)))
            {
                return false;
            }

            var name = HandlerSuffixPattern()
                .Replace(type.Name, string.Empty)
                .ToLowerCamelCase();

            builder.ModuleName = module.ToLowerCamelCase();
            builder.Route = subEntity != null
                ? $"/{CommandConstants.RoutePrefix}/{builder.ModuleName}/{subEntity.ToLowerCamelCase()}/{name}"
                : $"/{CommandConstants.RoutePrefix}/{builder.ModuleName}/{name}";
            
            return true;
        });
    }

    private static void ValidateDescriptor(CommandDescriptor descriptor)
    {
        if (ValidHandler(descriptor.MethodInfo))
        {
            return;
        }

        const string Signature = "Task<Response> Handle(Request request)";
        throw new ArgumentException(
            $"The type `{descriptor.CommandType.Name}` must have a public instance method of the `{Signature}` signature.",
            nameof(descriptor));
    }

    private static bool ValidHandler(MethodInfo? method) =>
        method != null &&
        method.GetParameters().Length > 0 &&
        method is { IsPrivate: false, IsStatic: false } &&
        method.ReturnType != typeof(void);

    private static string DefaultRoute(this Type commandType)
    {
        var typeName = commandType.FullName ?? commandType.Name;
        var nameParts = typeName.Split('.');
        var lastPart = nameParts.Last();
        var commandPart = lastPart.EndsWith(CommandConstants.CommandTypeSuffix)
            ? lastPart.Substring(0, lastPart.Length - CommandConstants.CommandTypeSuffix.Length)
            : lastPart;
        var parts = nameParts
            .TakeWhile(x => x != CommandConstants.CommandNamespacePart)
            .Concat(new[] { commandPart })
            .Select(x => x.ToLowerInvariant())
            .Prepend(CommandConstants.RoutePrefix);
        var route = string.Join('/', parts);
        return route;
    }

    [GeneratedRegex(CommandConstants.HandlerSuffixPattern)]
    private static partial Regex HandlerSuffixPattern();
}
