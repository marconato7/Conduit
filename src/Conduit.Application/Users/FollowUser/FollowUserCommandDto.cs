namespace Conduit.Application.Users.FollowUser;

public sealed record FollowUserCommandDto
(
    string  Username,
    string? Bio,
    string? Image,
    bool    Following
);
