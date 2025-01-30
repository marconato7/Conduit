namespace Conduit.Api.Controllers.Users.UpdateUser;

public sealed record UpdateUserResponseProps
(
    string  Email,
    string  Token,
    string  Username,
    string? Bio,
    string? Image
);
