using Bss.Infrastructure.Commands.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Bss.Infrastructure.Commands;

internal class ApiCommandDescriptionProvider(
    IOptions<MvcOptions> optionsAccessor,
    IInlineConstraintResolver constraintResolver,
    IModelMetadataProvider modelMetadataProvider,
    IActionResultTypeMapper mapper,
    IOptions<RouteOptions> routeOptions,
    IEnumerable<EndpointDataSource> endpointDataSources) : IApiDescriptionProvider
{
    private readonly DefaultApiDescriptionProvider _defaultApiDescriptionProvider = new(
        optionsAccessor,
        constraintResolver,
        modelMetadataProvider,
        mapper,
        routeOptions);
    private readonly IEnumerable<EndpointDataSource> _endpointDataSources = endpointDataSources;

    public int Order => 5000;

    public void OnProvidersExecuted(ApiDescriptionProviderContext context)
    {
        var commandEndpoints = _endpointDataSources
            .SelectMany(x => x.Endpoints)
            .Where(x => x is RouteEndpoint)
            .Cast<RouteEndpoint>()
            .Where(x => x.Metadata.Any(y => y is CommandHandlerAttribute))
            .ToArray();

        var actionDescriptors = commandEndpoints
            .Select(x => x.Metadata.FirstOrDefault(y => y is ControllerActionDescriptor))
            .Where(x => x != null)
            .Cast<ControllerActionDescriptor>()
            .GroupBy(x => x.MethodInfo)
            .Select(x => x.First())
            .ToList();

        var apiDescriptionProviderContext = new ApiDescriptionProviderContext(actionDescriptors);
        _defaultApiDescriptionProvider.OnProvidersExecuting(apiDescriptionProviderContext);
        _defaultApiDescriptionProvider.OnProvidersExecuted(apiDescriptionProviderContext);

        foreach (var apiDescription in apiDescriptionProviderContext.Results)
        {
            context.Results.Add(apiDescription);
        }
    }

    public void OnProvidersExecuting(ApiDescriptionProviderContext context)
    {
    }
}