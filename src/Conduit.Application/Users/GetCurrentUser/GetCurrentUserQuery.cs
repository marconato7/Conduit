using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Users.GetCurrentUser;

public sealed record GetCurrentUserQuery(string Email, string Token)
    : ICommand<GetCurrentUserQueryDto>;
