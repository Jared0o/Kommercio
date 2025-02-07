using Microsoft.AspNetCore.Identity;

namespace Kommercio.Modules.Users.Core.Entities;

public sealed class User : IdentityUser<Guid>
{
    public User()
    {
        Id = Guid.NewGuid();
    }
    
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
}