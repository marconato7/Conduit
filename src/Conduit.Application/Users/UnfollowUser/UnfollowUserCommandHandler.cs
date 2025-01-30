using Conduit.Application.Abstractions.Cqrs;
using Conduit.Application.Users.FollowUser;
using Conduit.Domain.Abstractions;
using Conduit.Domain.Users;
using FluentResults;

namespace Conduit.Application.Users.UnfollowUser;

internal sealed class UnfollowUserCommandHandler
(
    IUnitOfWork     unitOfWork,
    IUserRepository userRepository
) : ICommandHandler<UnfollowUserCommand, UnfollowUserCommandDto>
{
    private readonly IUnitOfWork     _unitOfWork     = unitOfWork;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<UnfollowUserCommandDto>> Handle
    (
        UnfollowUserCommand command,
        CancellationToken cancellationToken
    )
    {
        var currentUser = await _userRepository.GetByEmailAsync
        (
            command.CurrentUsersEmail,
            cancellationToken
        );

        var userToUnfollow = await _userRepository.GetByUsernameAsync
        (
            command.UsernameToUnfollow,
            cancellationToken
        );

        if (currentUser is null || userToUnfollow is null)
        {
            return Result.Fail("something went wrong");
        }

        currentUser.UnfollowUser(userToUnfollow);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UnfollowUserCommandDto
        (
            Username:  userToUnfollow.Username,
            Bio:       userToUnfollow.Bio,
            Image:     userToUnfollow.Image,
            Following: false
        );
    }
}
