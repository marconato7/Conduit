namespace Conduit.Application.Users.GetProfile;

public sealed record GetProfileQueryDto
(
    string  Username,
    string? Bio,
    string? Image,
    bool    Following
);
