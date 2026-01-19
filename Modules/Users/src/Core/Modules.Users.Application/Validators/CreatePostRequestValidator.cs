using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Modules.Users.Application.Dtos.Requests;

namespace Modules.Users.Application.Validators
{
    public class CreatePostRequestCollectionValidator : AbstractValidator<ICollection<CreatePostRequestDto>>
    {
        public CreatePostRequestCollectionValidator()
        {
            RuleForEach(x => x).SetValidator(new CreatePostRequestValidator());
            RuleFor(x => x)
                .NotEmpty().WithMessage("The collection must contain at least one post.")
                .Must(posts => posts.Select(p => p.Slug).Distinct().Count() == posts.Count)
                .WithMessage("Duplicate slugs found in the collection.");
        }
    }
    public class CreatePostRequestValidator : AbstractValidator<CreatePostRequestDto>
    {
        public CreatePostRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.");
            RuleFor(x => x.Tags)
                .Must(tags => tags.All(tag => !string.IsNullOrWhiteSpace(tag)))
                .WithMessage("Tags must not contain empty or whitespace-only strings.");
            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug is required.")
                .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug must be URL-friendly (lowercase letters, numbers, and hyphens only).");
        }
    }
}