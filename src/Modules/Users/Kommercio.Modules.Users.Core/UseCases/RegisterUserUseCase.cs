using FluentValidation;
using Kommercio.Modules.Users.Core.Dtos;
using Kommercio.Modules.Users.Core.Entities;
using Kommercio.Modules.Users.Core.Entities.Enums;
using Microsoft.AspNetCore.Identity;

namespace Kommercio.Modules.Users.Core.UseCases;

public class RegisterUserUseCase
{
    private readonly UserManager<User> _userManager;

    public RegisterUserUseCase(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task Execute(RegisterUser registerUser)
    {
        var validationResult = await new RegisterUserValidator().ValidateAsync(registerUser);

        if (!validationResult.IsValid)
            throw new ValidationException("Error validating the user registration", validationResult.Errors);
        
        var user = new User{Email = registerUser.Email, FirstName = registerUser.FirstName, LastName = registerUser.LastName, UserName = registerUser.Email};
        var password = registerUser.Password;
        var role = Roles.Member.ToString();

        var result = await _userManager.CreateAsync(user, password);
        if(!result.Succeeded)
            throw new Exception("Error creating the user"); // zmienić kiedyś 

        await _userManager.AddToRoleAsync(user, role);
    }
}