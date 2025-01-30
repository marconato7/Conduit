using Conduit.Application.Abstractions.Cqrs;
using Conduit.Domain.Users;
using FluentResults;

namespace Conduit.Application.Users.GetProfile;

internal sealed class GetProfileQueryHandler(IUserRepository userRepository)
: ICommandHandler<GetProfileQuery, GetProfileQueryDto>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<GetProfileQueryDto>> Handle
    (
        GetProfileQuery   query,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByUsernameAsync
        (
            query.Username,
            cancellationToken
        );

        if (user is null)
        {
            return Result.Fail("profile not found");
        }

        return new GetProfileQueryDto
        (
            Username:  user.Username,
            Bio:       user.Bio,
            Image:     user.Image,
            Following: query.Email is not null
        );
    }
}
