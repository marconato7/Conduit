using Conduit.Api.Data;
using Conduit.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Conduit.Api.Features.RegisterUser;

public static class RegisterUser
{
    internal sealed record Request(
        string Email,
        string Initials,
        string Password,
        bool EnableNotifications = false
    );

    public static void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost(
            "api/users/register",
            async (
                Request request,
                UserManager<ApplicationUser> userManager,
                ApplicationDbContext applicationDbContext
            ) =>
            {
                // using var transaction = await applicationDbContext.Database
                    // .BeginTransactionAsync();

                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    EnableNotifications = request.EnableNotifications,
                    Initials = request.Initials
                };

                var createResult = await userManager.
                    CreateAsync(user, request.Password);

                if (!createResult.Succeeded)
                {
                    return Results.BadRequest(createResult.Errors);
                }

                var addToRoleResult = await userManager.AddToRoleAsync(user, Roles.Member);
                if (!addToRoleResult.Succeeded)
                {
                    return Results.BadRequest(addToRoleResult.Errors);
                }

                // await transaction.CommitAsync();

                return Results.Ok(user);
            }
        );
    }
}
