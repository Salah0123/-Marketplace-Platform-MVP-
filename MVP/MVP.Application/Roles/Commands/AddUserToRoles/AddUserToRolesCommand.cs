using MediatR;
using MVP.Domain.Common;

namespace MVP.Application.Roles.Commands.AddUserToRoles;

public record AddUserToRolesCommand(string UserId, string[] Roles) : IRequest<Result>;