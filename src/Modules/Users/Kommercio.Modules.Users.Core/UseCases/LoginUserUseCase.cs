using FluentValidation;
using Kommercio.Modules.Users.Core.Dtos;
using Kommercio.Modules.Users.Core.Entities;
using Kommercio.Modules.Users.Core.Services;
using Microsoft.AspNetCore.Identity;

namespace Kommercio.Modules.Users.Core.UseCases;

public class LoginUserUseCase
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenGenerator _tokenGenerator;

    public LoginUserUseCase(UserManager<User> userManager, ITokenGenerator tokenGenerator)
    {
        _userManager = userManager;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<string> Execute(LoginUser loginUser)
    {
        var validationResult = await new LoginUserValidator().ValidateAsync(loginUser);

        if (!validationResult.IsValid)
            throw new ValidationException("Error validating the user login", validationResult.Errors);

        var user = await _userManager.FindByEmailAsync(loginUser.Email);
        if (user == null)
            throw new Exception("User not found");
        
        if (await _userManager.IsLockedOutAsync(user))
            throw new Exception("User is locked out");

        if (!await _userManager.CheckPasswordAsync(user, loginUser.Password))
        {
            await _userManager.AccessFailedAsync(user);
            throw new Exception("Invalid password");
        }

        await _userManager.ResetAccessFailedCountAsync(user);
        
        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenGenerator.GenerateToken(user, roles);

        return token;
    }
}