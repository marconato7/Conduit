using Conduit.Application.Abstractions.Cqrs;
using Conduit.Domain.Abstractions;
using Conduit.Domain.Users;
using FluentResults;

namespace Conduit.Application.Users.UpdateUser;

internal sealed class UpdateUserCommandHandler
(
    IUnitOfWork     unitOfWork,
    IUserRepository userRepository
) : ICommandHandler<UpdateUserCommand, UpdateUserCommandDto>
{
    private readonly IUnitOfWork     _unitOfWork     = unitOfWork;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<UpdateUserCommandDto>> Handle
    (
        UpdateUserCommand command,
        CancellationToken cancellationToken
    )
    {
        var userToUpdate = await _userRepository.GetByEmailAsync
        (
            command.CurrentUsersEmail,
            cancellationToken
        );

        if (userToUpdate is null)
        {
            throw new Exception(nameof(userToUpdate));
        }

        userToUpdate.Update
        (
            email:        command.Email,
            username:     command.Username,
            passwordHash: command.Password,
            bio:          command.Bio,
            image:        command.Image
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdateUserCommandDto
        (
            Email:    userToUpdate.Email,
            Token:    command.Token,
            Username: userToUpdate.Username,
            Bio:      userToUpdate.Bio,
            Image:    userToUpdate.Image
        );
    }
}
