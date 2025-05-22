using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

namespace SocialDietPlatform.Application.Features.Users.Commands.UpdateProfile;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad alanı boş bırakılamaz.")
            .MaximumLength(50).WithMessage("Ad en fazla 50 karakter olabilir.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad alanı boş bırakılamaz.")
            .MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olabilir.");

        RuleFor(x => x.Bio)
            .MaximumLength(500).WithMessage("Biyografi en fazla 500 karakter olabilir.")
            .When(x => !string.IsNullOrEmpty(x.Bio));

        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("Kilo 0'dan büyük olmalıdır.")
            .LessThan(1000).WithMessage("Kilo 1000'den küçük olmalıdır.")
            .When(x => x.Weight.HasValue);

        RuleFor(x => x.Height)
            .GreaterThan(0).WithMessage("Boy 0'dan büyük olmalıdır.")
            .LessThan(300).WithMessage("Boy 300'den küçük olmalıdır.")
            .When(x => x.Height.HasValue);

        RuleFor(x => x.TargetWeight)
            .GreaterThan(0).WithMessage("Hedef kilo 0'dan büyük olmalıdır.")
            .LessThan(1000).WithMessage("Hedef kilo 1000'den küçük olmalıdır.")
            .When(x => x.TargetWeight.HasValue);

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Today.AddYears(-13)).WithMessage("En az 13 yaşında olmalısınız.")
            .When(x => x.DateOfBirth.HasValue);
    }
}
