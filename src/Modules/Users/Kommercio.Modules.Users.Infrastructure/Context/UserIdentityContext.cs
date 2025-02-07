using Kommercio.Modules.Users.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kommercio.Modules.Users.Infrastructure.Context;

public class UserIdentityContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public UserIdentityContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("users");
        base.OnModelCreating(builder);
    }
}