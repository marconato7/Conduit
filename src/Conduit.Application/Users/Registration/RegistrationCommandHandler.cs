using Conduit.Application.Abstractions.Authentication;
using Conduit.Application.Abstractions.Cqrs;
using Conduit.Domain.Abstractions;
using Conduit.Domain.Users;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace Conduit.Application.Users.Registration;

internal sealed class RegistrationCommandHandler
(
    ITokenService   tokenService,
    IUnitOfWork     unitOfWork,
    IUserRepository userRepository
) : ICommandHandler<RegistrationCommand, RegistrationCommandDto>
{
    private readonly ITokenService   _tokenService   = tokenService;
    private readonly IUnitOfWork     _unitOfWork     = unitOfWork;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<RegistrationCommandDto>> Handle
    (
        RegistrationCommand command,
        CancellationToken cancellationToken
    )
    {
        var user = User.Create
        (
            username: command.Username,
            email:    command.Email
        );

        var passwordHash = new PasswordHasher<User>().HashPassword
        (
            user,
            command.Password
        );

        user.SetPasswordHash(passwordHash);

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var token = _tokenService.Create(user);

        return new RegistrationCommandDto
        (
            Email:    user.Email,
            Token:    token,
            Username: user.Username,
            Bio:      user.Bio,
            Image:    user.Image
        );
    }
}
