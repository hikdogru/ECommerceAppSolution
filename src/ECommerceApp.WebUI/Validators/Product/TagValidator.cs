using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.WebUI.Areas.Admin.Models.Catalog;
using FluentValidation;

namespace ECommerceApp.WebUI.Validators.Product;

public class TagValidator : AbstractValidator<TagModel>
{
    public TagValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
    }
}
