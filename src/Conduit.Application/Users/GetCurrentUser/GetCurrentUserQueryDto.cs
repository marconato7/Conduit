namespace Conduit.Application.Users.GetCurrentUser;

public sealed record GetCurrentUserQueryDto
(
    string Email,
    string Token,
    string Username,
    string? Bio,
    string? Image
);
