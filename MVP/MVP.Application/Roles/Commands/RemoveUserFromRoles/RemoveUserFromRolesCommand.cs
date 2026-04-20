using MediatR;
using MVP.Domain.Common;

namespace MVP.Application.Roles.Commands.RemoveUserFromRoles;

public record RemoveUserFromRolesCommand(string UserId, string[] Roles) : IRequest<Result>;
