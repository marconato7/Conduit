using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Users.Authentication;

public sealed record AuthenticationCommand
(
    string Email,
    string Password
) : ICommand<AuthenticationCommandDto>;
