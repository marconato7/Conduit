using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Infrastructure.Authentication;

internal static class AuthenticationExtensions
{
    internal static IServiceCollection AddAuthenticationIternal(this IServiceCollection services)
    {
        services.AddAuthentication().AddJwtBearer();

        services.AddAuthorization();

        services.AddHttpContextAccessor();

        services.ConfigureOptions<JwtBearerConfigureOptions>();

        return services;
    }
}
