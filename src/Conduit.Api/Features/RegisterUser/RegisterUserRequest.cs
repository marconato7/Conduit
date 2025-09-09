namespace Conduit.Api.Features.RegisterUser;

internal sealed record RegisterUserRequest(
    string Email,
    string Initials,
    string Password,
    bool   EnableNotifications = false
);
