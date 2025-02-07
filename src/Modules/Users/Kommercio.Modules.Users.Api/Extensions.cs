using Kommercio.Modules.Users.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kommercio.Modules.Users.Api;

public static class Extensions
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration config)
    {
        services.AddUserInfrastructure(config);
        return services;
    }
}