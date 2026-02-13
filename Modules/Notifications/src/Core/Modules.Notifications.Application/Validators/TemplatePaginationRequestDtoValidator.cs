using Common.Application.Dtos;
using FluentValidation;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Application.Validators
{
    public class TemplatePaginationDtoValidator : AbstractValidator<PaginationDto<Template>>
    {
        private const int MaxPageSize = 50;

        public TemplatePaginationDtoValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page must be at least 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageSize must be at least 1.")
                .LessThanOrEqualTo(MaxPageSize)
                .WithMessage($"PageSize cannot be more than {MaxPageSize}.");
        }
    }

}
