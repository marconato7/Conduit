using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Users.UpdateUser;

public sealed record UpdateUserCommand
(
    string  CurrentUsersEmail,
    string  Token,
    string? Email,
    string? Username,
    string? Password,
    string? Image,
    string? Bio
) : ICommand<UpdateUserCommandDto>;
