using Conduit.Application.Abstractions.Authentication;
using Conduit.Application.Abstractions.Clock;
using Conduit.Application.Abstractions.Data;
using Conduit.Domain.Abstractions;
using Conduit.Domain.Articles;
using Conduit.Domain.Users;
using Conduit.Infrastructure.Authentication;
using Conduit.Infrastructure.Clock;
using Conduit.Infrastructure.Data;
using Conduit.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure
    (
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("Database")
            ?? throw new Exception(nameof(configuration));

        services.AddSingleton<ITokenService, TokenService>();

        // services.AddAuthenticationIternal();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        });

        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
