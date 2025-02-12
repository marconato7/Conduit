using System.Reflection;
using System.Text;
using Conduit.Api.Extensions;
using Conduit.Application;
using Conduit.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme             = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata      = false;
        options.SaveToken                 = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey              = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Secret").Value!)),
            ValidateAudience              = false,
            ValidateIssuer                = false,
            ValidateIssuerSigningKey      = false,
            ValidAudience                 = builder.Configuration.GetSection("Jwt:Audience").Value!,
            ValidIssuer                   = builder.Configuration.GetSection("Jwt:Issuer").Value!,
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                if (ctx.Request.Headers.ContainsKey("Authorization"))
                {
                    var bearerToken = ctx.Request.Headers.Authorization.ElementAt(0);
                    var token = bearerToken!.StartsWith("Token ") ? bearerToken[6..] : bearerToken;
                    ctx.Token = token;
                }

                return Task.CompletedTask;
            }
        };
    })
    .AddBearerToken();

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

// builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
builder.Services.AddEndpoints(typeof(Program).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // app.ApplyMigrations();
    // app.SeedData();
}

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

var port = Environment.GetEnvironmentVariable("PORT");

app.MapEndpoints();

app.Run();

// app.Run($"http://localhost:{port}");

// app.Urls.Add("http://localhost:6666");
// app.Urls.Add("https://localhost:6667");

// app.Run("http://localhost:7070");
// app.Run("https://localhost:7071");

// var builder = WebApplication.CreateBuilder(args);

// var app = builder.Build();

// app.MapGet("/", (ILogger<Program> logger) =>
// {
//     logger.LogInformation("Hello, World!");

//     return "Hello, World!";
// });
