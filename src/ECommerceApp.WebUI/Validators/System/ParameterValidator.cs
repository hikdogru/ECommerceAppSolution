using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.WebUI.Areas.Admin.Models.System;
using FluentValidation;

namespace ECommerceApp.WebUI.Validators.System;

public class ParameterValidator : AbstractValidator<ParameterModel>
{
    public ParameterValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("The name field is required!");
        RuleFor(x => x.Value).NotEmpty().WithMessage("The value field is required!");
    }
}
