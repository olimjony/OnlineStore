using FluentValidation;
using OnlineStore.Application.DTOs;
namespace OnlineStore.Application.Validators;

public class MarketplaceValidator : AbstractValidator<CreateMarketplaceDTO>
{
    public MarketplaceValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name of your marketplace is required!");

        RuleFor(x => x.Description)
            .MaximumLength(200).WithMessage("The length of description have to be shorter than 200 symbols!");

        /*RuleFor(x => x.ImageURL)
            .Must(BeAValidUrl).WithMessage("Invalid URL format!");*/
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) 
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
