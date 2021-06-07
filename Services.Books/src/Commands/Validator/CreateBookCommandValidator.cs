using FluentValidation;

namespace Services.Books.Commands.Validator
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().NotNull();
            RuleFor(c => c.Price).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(c => c.Quantity).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
