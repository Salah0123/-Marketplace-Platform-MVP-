using FluentValidation;

namespace MVP.Application.ServiceRequests.Commands.AcceptRequest;


public class AcceptRequestCommandValidator : AbstractValidator<AcceptRequestCommand>
{
    public AcceptRequestCommandValidator()
    {
        RuleFor(x=>x.Id)
            .NotEmpty()
            .WithMessage("id is required")
            .GreaterThan(0)
            .WithMessage("id must be greater than 0");
        RuleFor(x => x.ProvuderId)
            .NotEmpty()
            .WithMessage("ProvuderId is required.")
            .MaximumLength(450)
            .WithMessage("ProvuderId must not exceed 450 characters.");
        
    }
}