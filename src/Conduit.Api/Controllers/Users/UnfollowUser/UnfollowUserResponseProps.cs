namespace Conduit.Api.Controllers.Users.UnfollowUser;

public sealed record UnfollowUserResponseProps(string Username, string? Bio, string? Image, bool Following);
