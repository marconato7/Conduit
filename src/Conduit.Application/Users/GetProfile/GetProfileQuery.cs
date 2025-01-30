using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Users.GetProfile;

public sealed record GetProfileQuery(string Username, string? Email)
    : ICommand<GetProfileQueryDto>;
