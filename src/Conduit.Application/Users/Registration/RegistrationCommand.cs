using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Users.Registration;

public sealed record RegistrationCommand
(
    string Username,
    string Email,
    string Password
) : ICommand<RegistrationCommandDto>;
