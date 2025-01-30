namespace Conduit.Api.Controllers.Users.Authentication;

public sealed record AuthenticationResponseProps(string Email, string Token, string Username, string? Bio, string? Image);
