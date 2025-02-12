namespace Conduit.Application.Articles.Shared;

public sealed record AuthorModel
(
    string  Username,
    string? Bio,
    string? Image,
    bool    Following
);
