using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

namespace SocialDietPlatform.Application.Features.Posts.Commands.CreatePost;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Gönderi içeriği boş bırakılamaz.")
            .MaximumLength(2000).WithMessage("Gönderi içeriği en fazla 2000 karakter olabilir.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Kullanıcı ID'si gereklidir.");

        RuleFor(x => x.ImageUrl)
            .Must(BeValidUrl).WithMessage("Geçerli bir resim URL'si giriniz.")
            .When(x => !string.IsNullOrEmpty(x.ImageUrl));

        RuleFor(x => x.VideoUrl)
            .Must(BeValidUrl).WithMessage("Geçerli bir video URL'si giriniz.")
            .When(x => !string.IsNullOrEmpty(x.VideoUrl));
    }

    private bool BeValidUrl(string? url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}