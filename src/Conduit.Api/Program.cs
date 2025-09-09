using Conduit.Api.Data;
using Conduit.Api.Exceptions;
using Conduit.Api.Extensions;
using Conduit.Api.Features.GetTags;
using Conduit.Api.Features.RegisterUser;
using Conduit.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider(config =>
{
    config.ValidateOnBuild = true;
});

builder.WebHost.UseKestrel(options => options.AddServerHeader = false);

// builder.Services.AddAuthorizationBuilder()
//     .SetFallbackPolicy(new AuthorizationPolicyBuilder()
//     .RequireAuthenticatedUser()
//     .Build());

builder.Services.AddProblemDetails(configure =>
{
    configure.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions.TryAdd(
            "requestId", context.HttpContext.TraceIdentifier
        );
    };
});

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // options.UseSqlite("conduit.api.users.sqlite");
    // options.UseInMemoryDatabase("conduit.api.users");
    options
        .LogTo(Console.WriteLine)
        .UseNpgsql(builder.Configuration.GetConnectionString("Database"))
        .UseSnakeCaseNamingConvention();
});

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// builder.Services
//     .AddAuthentication(options =>
//     {
//         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//         options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
//         options.DefaultScheme             = JwtBearerDefaults.AuthenticationScheme;
//     })
//     .AddJwtBearer(options =>
//     {
//         options.RequireHttpsMetadata      = false;
//         options.SaveToken                 = true;
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             IssuerSigningKey              = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Secret").Value!)),
//             ValidateAudience              = false,
//             ValidateIssuer                = false,
//             ValidateIssuerSigningKey      = false,
//             ValidAudience                 = builder.Configuration.GetSection("Jwt:Audience").Value!,
//             ValidIssuer                   = builder.Configuration.GetSection("Jwt:Issuer").Value!,
//         };
//         options.Events = new JwtBearerEvents
//         {
//             OnMessageReceived = ctx =>
//             {
//                 if (ctx.Request.Headers.ContainsKey("Authorization"))
//                 {
//                     var bearerToken = ctx.Request.Headers.Authorization.ElementAt(0);
//                     var token = bearerToken!.StartsWith("Token ") ? bearerToken[6..] : bearerToken;
//                     ctx.Token = token;
//                 }

//                 return Task.CompletedTask;
//             }
//         };
//     })
//     .AddBearerToken();

// builder.Services.AddAuthorization();

builder.Services.AddControllers();

// builder.Services.AddApplication(builder.Configuration);
// builder.Services.AddInfrastructure(builder.Configuration);

// builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
// builder.Services.AddEndpoints(typeof(Program).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.ApplyMigrations();
    // app.SeedData();
}

app.MapControllers();

app.UseExceptionHandler();

// app.UseAuthentication();
// app.UseAuthorization();

// app.MapEndpoints();

GetTags.MapEndpoint(app);
RegisterUser.MapEndpoint(app);

// app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();

await app.RunAsync();
