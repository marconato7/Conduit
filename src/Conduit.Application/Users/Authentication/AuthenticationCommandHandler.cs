using Conduit.Application.Abstractions.Authentication;
using Conduit.Application.Abstractions.Cqrs;
using Conduit.Domain.Abstractions;
using Conduit.Domain.Users;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace Conduit.Application.Users.Authentication;

internal sealed class AuthenticationCommandHandler
(
    ITokenService   tokenService,
    IUnitOfWork     unitOfWork,
    IUserRepository userRepository
) : ICommandHandler<AuthenticationCommand, AuthenticationCommandDto>
{
    private readonly ITokenService   _tokenService   = tokenService;
    private readonly IUnitOfWork     _unitOfWork     = unitOfWork;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<AuthenticationCommandDto>> Handle
    (
        AuthenticationCommand command,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (user is null)
        {
            return Result.Fail("authentication failed");
        }

        // refactor: inject
        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, command.Password) == PasswordVerificationResult.Failed)
        {
            return Result.Fail("authentication failed");
        }

        var token = _tokenService.Create(user);
        if (token is null)
        {
            return Result.Fail("authentication failed");
        }

        return new AuthenticationCommandDto
        (
            Email:    user.Email,
            Token:    token,
            Username: user.Username,
            Bio:      user.Bio,
            Image:    user.Image
        );
    }
}
