namespace Conduit.Api.Controllers.Users.GetProfile;

public sealed record GetProfileResponseProps(string Username, string? Bio, string? Image, bool Following);
