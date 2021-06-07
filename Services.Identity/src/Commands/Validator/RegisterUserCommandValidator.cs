using FluentValidation;

namespace Services.Identity.Commands.Validator
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(c => c.Email).NotEmpty().NotNull().EmailAddress();
            RuleFor(c => c.FirstName).NotEmpty().NotNull();
            RuleFor(c => c.LastName).NotEmpty().NotNull();
            RuleFor(c => c.Password).NotEmpty().NotNull();
            RuleFor(c => c.Address).NotEmpty().NotNull();
        }
    }
}
