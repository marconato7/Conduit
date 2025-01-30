namespace Conduit.Api.Controllers.Users.GetCurrentUser;

public sealed record GetCurrentUserResponseProps(string Email, string Token, string Username, string? Bio, string? Image);
