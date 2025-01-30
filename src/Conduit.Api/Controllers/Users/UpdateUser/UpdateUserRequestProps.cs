namespace Conduit.Api.Controllers.Users.UpdateUser;

public sealed record UpdateUserRequestProps
(
    string? Email,
    string? Username,
    string? Password,
    string? Image,
    string? Bio
);
