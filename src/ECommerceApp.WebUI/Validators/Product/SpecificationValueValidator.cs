using ECommerceApp.WebUI.Areas.Admin.Models.Catalog;
using FluentValidation;

namespace ECommerceApp.WebUI.Validators.Product;

public class SpecificationValueValidator : AbstractValidator<SpecificationValueModel>
{
    public SpecificationValueValidator()
    {
        RuleFor(x => x.SpecificationValueLanguages)
           .Must((model, specLanguages) => specLanguages.Any(sl => !string.IsNullOrEmpty(sl.Value)))
           .WithMessage((model, specLanguages) =>
               $"At least one Value field in {model.GetType().Name}.SpecificationValueLanguages must not be null.")
           .WithName("Value");
    }
}
