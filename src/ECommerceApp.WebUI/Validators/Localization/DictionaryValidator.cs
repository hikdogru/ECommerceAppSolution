using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.WebUI.Areas.Admin.Models.Localization;
using FluentValidation;

namespace ECommerceApp.WebUI.Validators.Localization
{
    public class DictionaryValidator : AbstractValidator<DictionaryModel>
    {
        public DictionaryValidator()
        {
            RuleFor(l => l.Key).NotEmpty().WithMessage("Key is required!");
            RuleFor(l => l.Value).NotEmpty().WithMessage("Value is required!");
            RuleFor(l => l.LanguageId).NotEmpty().WithMessage("Language is required!");
        }
    }
}