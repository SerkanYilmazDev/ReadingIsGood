using FluentValidation;

namespace Services.Customers.Commands.Validator
{
    public class AddBookToBasketCommandValidator : AbstractValidator<AddBookToBasketCommand>
    {
        public AddBookToBasketCommandValidator()
        {
            RuleFor(c => c.Quantity).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
