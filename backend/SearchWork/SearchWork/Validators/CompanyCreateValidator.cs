using FluentValidation;
using SearchWork.Models.DTO;

namespace SearchWork.Validators
{
    public class CompanyCreateValidator : AbstractValidator<CompanyCreateDTO>
    {
        public CompanyCreateValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название компании обязательно")
            .MaximumLength(100).WithMessage("Название компании не должно превышать 100 символов");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Описание компании обязательно")
                .MaximumLength(1000).WithMessage("Описание компании не должно превышать 1000 символов");
        }
        
    }
}
