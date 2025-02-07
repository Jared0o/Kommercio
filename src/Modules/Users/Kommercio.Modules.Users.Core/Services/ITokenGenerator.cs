using Kommercio.Modules.Users.Core.Entities;

namespace Kommercio.Modules.Users.Core.Services;

public interface ITokenGenerator
{
    public string GenerateToken(User user, IList<string> roles);
}