using ECommerceApp.WebUI.Areas.Admin.Models.Localization;
using FluentValidation;

namespace ECommerceApp.WebUI.Validators.Localization;

public class LanguageValidator : AbstractValidator<LanguageModel>
{
    public LanguageValidator()
    {
        RuleFor(l => l.Name).NotEmpty().WithMessage("Name is required!");
        RuleFor(l => l.Code).NotEmpty().WithMessage("Code is required!");
        RuleFor(l => l.Direction).NotEmpty().WithMessage("Direction is required!");
    }
}