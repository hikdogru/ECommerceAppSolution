using ECommerceApp.WebUI.Models.Category;
using FluentValidation;

namespace ECommerceApp.WebUI.Validators.Product;

public class CategoryValidator : AbstractValidator<CategoryModel>
{
    public CategoryValidator()
    {
        RuleForEach(x => x.CategoryMedias).Where(x => x.File != null).SetValidator(new CategoryMediaValidator());

        RuleForEach(x => x.CategoryMedias).NotNull()
            .When(x => x.CategoryMedias.All(m => m.File == null))
            .WithMessage("Category media cannot be null!");

        RuleFor(x => x.CategoryLanguages).NotEmpty()
            .When(x => x.CategoryLanguages!.Count == 0)
            .WithMessage("Category language cannot be empty!");
    }
}

public class CategoryMediaValidator : AbstractValidator<CategoryMediaModel>
{
    public CategoryMediaValidator()
    {
        RuleFor(x => x.File).NotNull().WithMessage("File cannot be null");
        RuleFor(x => x.LanguageId).NotNull().WithMessage("Language cannot be null");
        RuleFor(x => x.LanguageCode).NotNull().WithMessage("Language code cannot be null");
        RuleFor(x => x.MediaType).NotNull().WithMessage("Media type cannot be null");
        RuleFor(x => x.SizeType).NotNull().WithMessage("Size type cannot be null");
    }
}