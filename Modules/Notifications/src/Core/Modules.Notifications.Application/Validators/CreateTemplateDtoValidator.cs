using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Modules.Notifications.Application.Dtos.Requests;

namespace Modules.Notifications.Application.Validators
{
    public class CreateTemplateDtoValidator : AbstractValidator<CreateTemplateDto>
    {
        public CreateTemplateDtoValidator()
        {
            RuleFor(x => x.ContentType)
                .IsInEnum()
                .WithMessage("Invalid content Type.");

            RuleFor(x => x.TemplateType)
                .IsInEnum()
                .WithMessage("Invalid Template Type.");

            RuleForEach(x => x.Translations).ChildRules(lang =>
            {
                lang.RuleFor(l => l.LangCode)
                    .Matches("^[a-z]{2}$")
                    .Must(x => !string.IsNullOrWhiteSpace(x))
                    .WithMessage("Language is required.");

                lang.RuleFor(l => l.Title)
                    .MaximumLength(50)
                    .Must(x => !string.IsNullOrWhiteSpace(x));

                lang.RuleFor(l => l.Content)
                    .MaximumLength(500)
                    .Must(x => !string.IsNullOrWhiteSpace(x));
            });
        }
    }

}
