using ECommerceApp.WebUI.Areas.Admin.Models.Catalog;
using FluentValidation;

namespace ECommerceApp.WebUI.Validators.Product;

public class SpecificationValidator : AbstractValidator<SpecificationModel>
{
    public SpecificationValidator()
    {
        RuleFor(x => x.SpecificationLanguages)
            .Must((model, specLanguages) => specLanguages.Any(sl => !string.IsNullOrEmpty(sl.Name)))
            .WithMessage((model, specLanguages) =>
                $"At least one Name field in {model.GetType().Name}.SpecificationLanguages must not be null.")
            .WithName("Name");
    }
}


