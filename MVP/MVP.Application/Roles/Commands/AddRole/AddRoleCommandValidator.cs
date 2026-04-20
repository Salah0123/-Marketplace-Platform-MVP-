using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVP.Application.Roles.Commands.AddRole;

public class AddRoleCommandValidator : AbstractValidator<AddRoleCommand>
{
    public AddRoleCommandValidator()
    {
        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required");
        RuleFor(x => x.RoleKey)
            .NotEmpty().WithMessage("RoleKey is required");
    }
}
