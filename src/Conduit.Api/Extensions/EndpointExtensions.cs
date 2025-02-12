using System.Reflection;
using Conduit.Api.Endpoints;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Conduit.Api.Extensions;

// internal static class EndpointExtensions
// {
//     public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
//     {
//         ServiceDescriptor[] endpointServiceDescriptors = assembly
//             .DefinedTypes
//             .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableFrom(typeof(IEndpoint)))
//             .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
//             .ToArray();

//         services.TryAddEnumerable(endpointServiceDescriptors);

//         return services;
//     }

//     public static IApplicationBuilder MapEndpoints(this WebApplication app)
//     {
//         IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

//         foreach (IEndpoint endpoint in endpoints)
//         {
//             endpoint.MapEndpoint(app);
//         }

//         return app;
//     }
// }

internal static class EndpointExtensions
{
internal static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
{
    ServiceDescriptor[] serviceDescriptors = assembly
        .DefinedTypes
        .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                       type.IsAssignableTo(typeof(IEndpoint)))
        .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
        .ToArray();

    services.TryAddEnumerable(serviceDescriptors);

    return services;
}

public static IApplicationBuilder MapEndpoints(
    this WebApplication app,
    RouteGroupBuilder? routeGroupBuilder = null)
{
    IEnumerable<IEndpoint> endpoints = app.Services
        .GetRequiredService<IEnumerable<IEndpoint>>();

    IEndpointRouteBuilder builder =
        routeGroupBuilder is null ? app : routeGroupBuilder;

    foreach (IEndpoint endpoint in endpoints)
    {
        endpoint.MapEndpoint(builder);
    }

    return app;
}
}
