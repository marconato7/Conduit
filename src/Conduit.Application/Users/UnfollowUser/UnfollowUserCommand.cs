using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Users.UnfollowUser;

public sealed record UnfollowUserCommand
(
    string UsernameToUnfollow,
    string CurrentUsersEmail
) : ICommand<UnfollowUserCommandDto>;
