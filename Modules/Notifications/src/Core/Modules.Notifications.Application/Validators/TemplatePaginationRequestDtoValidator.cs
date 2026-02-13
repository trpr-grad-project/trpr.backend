using Common.Application.Dtos;
using FluentValidation;
using Modules.Notifications.Application.Dtos.Requests;

namespace Modules.Notifications.Application.Validators
{
    public class PaginateRequestDtoValidator : AbstractValidator<PaginateRequestDto>
    {

        public PaginateRequestDtoValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page must be at least 1.");
        }
    }

}
