using FluentValidation;

namespace MVP.Application.ServiceRequests.Commands.CreateServiceRequest;

public class CreateServiceRequestCommandValidator : AbstractValidator<CreateServiceRequestCommand>
{
    public CreateServiceRequestCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(100)
            .WithMessage("Title must not exceed 100 characters.");
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");


        // 📍 Latitude
        RuleFor(x => x.Latitude)
            .NotEmpty().WithMessage("Latitude is required.")
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90.");

        // 📍 Longitude
        RuleFor(x => x.Longitude)
            .NotEmpty().WithMessage("Longitude is required.")
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180.");


    }
}
