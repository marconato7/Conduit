namespace Conduit.Api;

public sealed record AuthorModelForResponse
(
    string  Username,
    string? Bio,
    string? Image,
    bool    Following
);
