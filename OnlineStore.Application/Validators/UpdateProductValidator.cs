using FluentValidation;
using OnlineStore.Application.DTOs;

namespace OnlineStore.Application.Validators;

public class UpdateProductValidator : AbstractValidator<UpdateProductDTO>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50).WithMessage("The length should be less than 50 symbols!")
            .NotEmpty().WithMessage("Name of the Product is required");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("It Actually can be zero!");
    }
}
