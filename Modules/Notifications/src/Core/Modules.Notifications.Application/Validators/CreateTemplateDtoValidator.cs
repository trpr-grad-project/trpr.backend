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
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required.");

            RuleFor(x => x.TemplateType)
                .IsInEnum()
                .WithMessage("Invalid template type.");

            RuleFor(x => x.ContentType)
                .IsInEnum()
                .WithMessage("Invalid content type.");
        }
    }

}
