namespace Conduit.Application.Users.Authentication;

public sealed record AuthenticationCommandDto
(
    string Email,
    string Token,
    string Username,
    string? Bio,
    string? Image
);
