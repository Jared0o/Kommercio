using FluentValidation;

namespace Kommercio.Modules.Users.Core.Dtos;

public class LoginUser
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class LoginUserValidator : AbstractValidator<LoginUser>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().NotNull();
    }
}