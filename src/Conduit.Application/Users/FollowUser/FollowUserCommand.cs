using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Users.FollowUser;

public sealed record FollowUserCommand
(
    string UsernameToFollow,
    string CurrentUsersEmail
) : ICommand<FollowUserCommandDto>;
