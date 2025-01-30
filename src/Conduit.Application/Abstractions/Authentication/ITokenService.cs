using Conduit.Domain.Users;

namespace Conduit.Application.Abstractions.Authentication;

public interface ITokenService
{
    public string Create(User user);
}
