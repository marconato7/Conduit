using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication
    (
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        return services;
    }
}
