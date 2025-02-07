using FluentValidation;

namespace Kommercio.Modules.Users.Core.Dtos;

public class RegisterUser
{
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Password { get; set; }
}

public class RegisterUserValidator : AbstractValidator<RegisterUser>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().NotNull();
    }
}