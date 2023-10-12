using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.WebUI.Areas.Admin.Models.Catalog;
using FluentValidation;

namespace ECommerceApp.WebUI.Validators.Product;

public class BrandValidator : AbstractValidator<BrandModel>
{
    public BrandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
    }
}
