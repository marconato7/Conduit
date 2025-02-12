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
        var existingUser = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (existingUser is not null)
        {
            // refactor: inject
            if(
                new PasswordHasher<User>().VerifyHashedPassword
                (
                    existingUser,
                    existingUser.PasswordHash,
                    command.Password
                ) == PasswordVerificationResult.Failed
            )
            {
                return Result.Fail("authentication failed");
            }

            var tokenForExistingUser = _tokenService.Create(existingUser);
            if (tokenForExistingUser is null)
            {
                return Result.Fail("authentication failed");
            }

            return new RegistrationCommandDto
            (
                Email:    existingUser.Email,
                Token:    tokenForExistingUser,
                Username: existingUser.Username,
                Bio:      existingUser.Bio,
                Image:    existingUser.Image
            );
        }

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
