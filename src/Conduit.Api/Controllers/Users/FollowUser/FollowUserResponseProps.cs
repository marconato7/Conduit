namespace Conduit.Api.Controllers.Users.FollowUser;

public sealed record FollowUserResponseProps(string Username, string? Bio, string? Image, bool Following);
