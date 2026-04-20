using MediatR;
using MVP.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVP.Application.Roles.Commands.AddRole;

public record AddRoleCommand(string Role, string RoleKey, string DisplayName) : IRequest<Result>;
