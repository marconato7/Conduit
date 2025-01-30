using Conduit.Application.Abstractions.Authentication;
using Conduit.Application.Abstractions.Cqrs;
using Conduit.Domain.Abstractions;
using Conduit.Domain.Users;
using FluentResults;

namespace Conduit.Application.Users.GetCurrentUser;

internal sealed class GetCurrentUserQueryHandler
(
    ITokenService   tokenService,
    IUnitOfWork     unitOfWork,
    IUserRepository userRepository
) : ICommandHandler<GetCurrentUserQuery, GetCurrentUserQueryDto>
{
    private readonly ITokenService   _tokenService   = tokenService;
    private readonly IUnitOfWork     _unitOfWork     = unitOfWork;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<GetCurrentUserQueryDto>> Handle
    (
        GetCurrentUserQuery query,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByEmailAsync(query.Email, cancellationToken);
        if (user is null)
        {
            return Result.Fail("authentication failed");
        }

        return new GetCurrentUserQueryDto
        (
            Email:    user.Email,
            Token:    query.Token,
            Username: user.Username,
            Bio:      user.Bio,
            Image:    user.Image
        );
    }
}
