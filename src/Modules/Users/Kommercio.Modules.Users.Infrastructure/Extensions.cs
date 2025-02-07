using System.Text;
using Kommercio.Modules.Users.Core.Entities;
using Kommercio.Modules.Users.Core.Services;
using Kommercio.Modules.Users.Core.UseCases;
using Kommercio.Modules.Users.Infrastructure.Context;
using Kommercio.Modules.Users.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Kommercio.Modules.Users.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddUserInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ITokenGenerator, TokenGenerator>();
        services.AddTransient<RegisterUserUseCase>();
        services.AddTransient<LoginUserUseCase>();
        
        services.AddDbContext<UserIdentityContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("KommercioContext"));
        });
        
        services.AddIdentityCore<User>()
            .AddRoles<IdentityRole<Guid>>()
            .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
            .AddUserManager<UserManager<User>>()
            .AddEntityFrameworkStores<UserIdentityContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(opt =>
        {
            opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(365 * 1000); // 1000 lat
            opt.Lockout.MaxFailedAccessAttempts = 10;
            opt.Lockout.AllowedForNewUsers = true;
        });
        
        
        var jwtKey = configuration["Jwt:Key"]!;
        var jwtIssuer = configuration["Jwt:Issuer"]!;

        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });
        services.AddAuthorization();
        
        return services;
    }
}