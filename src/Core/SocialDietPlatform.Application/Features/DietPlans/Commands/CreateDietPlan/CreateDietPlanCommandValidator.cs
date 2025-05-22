using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

namespace SocialDietPlatform.Application.Features.DietPlans.Commands.CreateDietPlan;

public class CreateDietPlanCommandValidator : AbstractValidator<CreateDietPlanCommand>
{
    public CreateDietPlanCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Plan adı boş bırakılamaz.")
            .MaximumLength(200).WithMessage("Plan adı en fazla 200 karakter olabilir.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Başlangıç tarihi boş bırakılamaz.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("Bitiş tarihi boş bırakılamaz.")
            .GreaterThan(x => x.StartDate).WithMessage("Bitiş tarihi başlangıç tarihinden sonra olmalıdır.");

        RuleFor(x => x.TargetCalories)
            .GreaterThan(0).WithMessage("Hedef kalori 0'dan büyük olmalıdır.")
            .LessThan(10000).WithMessage("Hedef kalori 10000'den küçük olmalıdır.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Kullanıcı ID'si gereklidir.");
    }
}