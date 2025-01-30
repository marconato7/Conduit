namespace Conduit.Application.Users.Registration;

public sealed record RegistrationCommandDto
(
    string Email,
    string Token,
    string Username,
    string? Bio,
    string? Image
);
