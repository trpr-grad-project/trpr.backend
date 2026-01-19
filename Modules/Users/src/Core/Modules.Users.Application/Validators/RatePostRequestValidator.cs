using FluentValidation;
using Modules.Users.Application.Dtos.Requests;

namespace Modules.Users.Application.Validators;

public class RatePostRequestValidator : AbstractValidator<RatePostRequestDto>
{
    public RatePostRequestValidator()
    {
        RuleFor(x => x.Rating)
            .InclusiveBetween((byte)1, (byte)5)
            .WithMessage("Rating must be between 1 and 5.");
    }
}