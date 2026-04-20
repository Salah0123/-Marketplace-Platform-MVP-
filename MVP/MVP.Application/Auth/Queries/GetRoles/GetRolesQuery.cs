using MediatR;
using MVP.Domain.Common;

namespace MVP.Application.Auth.Queries.GetRoles;

public record GetRolesQuery() : IRequest<Result<IEnumerable<RoleDto>>>;
