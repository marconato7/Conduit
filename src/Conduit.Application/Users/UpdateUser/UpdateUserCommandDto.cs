namespace Conduit.Application.Users.UpdateUser;

public sealed record UpdateUserCommandDto
(
    string  Email,
    string  Token,
    string  Username,
    string? Bio,
    string? Image
);
