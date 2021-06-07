using FluentValidation;

namespace Services.Books.Commands.Validator
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            RuleFor(c => c.Id).NotNull();
            RuleFor(c => c.Price).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(c => c.Quantity).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
