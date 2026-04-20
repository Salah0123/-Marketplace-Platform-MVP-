using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVP.Application.Auth.Queries.GetToken;

public class GetTokenQueryValidator : AbstractValidator<GetTokenQuery>
{
    public GetTokenQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Username is required");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}
