using Conduit.Application.Abstractions.Cqrs;
using Conduit.Domain.Abstractions;
using Conduit.Domain.Users;
using FluentResults;

namespace Conduit.Application.Users.FollowUser;

internal sealed class FollowUserCommandHandler
(
    IUnitOfWork     unitOfWork,
    IUserRepository userRepository
) : ICommandHandler<FollowUserCommand, FollowUserCommandDto>
{
    private readonly IUnitOfWork     _unitOfWork     = unitOfWork;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<FollowUserCommandDto>> Handle
    (
        FollowUserCommand command,
        CancellationToken cancellationToken
    )
    {
        var currentUser = await _userRepository.GetByEmailAsync
        (
            command.CurrentUsersEmail,
            cancellationToken
        );

        var userToFollow = await _userRepository.GetByUsernameAsync
        (
            command.UsernameToFollow,
            cancellationToken
        );

        if (currentUser is null || userToFollow is null)
        {
            return Result.Fail("something went wrong");
        }

        currentUser.FollowUser(userToFollow);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new FollowUserCommandDto
        (
            Username:  userToFollow.Username,
            Bio:       userToFollow.Bio,
            Image:     userToFollow.Image,
            Following: true
        );
    }
}
