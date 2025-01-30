namespace Conduit.Application.Users.UnfollowUser;

public sealed record UnfollowUserCommandDto
(
    string  Username,
    string? Bio,
    string? Image,
    bool    Following
);
