using MediatR;
using Microsoft.AspNetCore.Identity;
using MVP.Domain.Common;
using MVP.Domain.Entities;
using MVP.Domain.Enums;
using MVP.Services.IServices;

namespace MVP.Application.Auth.Queries.GetRoles;


public class GetRolesQueryHandler(IAuthService authService, RoleManager<ApplicationRole> roleManager) : IRequestHandler<GetRolesQuery, Result<IEnumerable<RoleDto>>>
{
    private readonly IAuthService _authService = authService;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

    public async Task<Result<IEnumerable<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = _roleManager.Roles
        .Select(r => new RoleDto
        {
            RoleKey = r.Name ?? "",
            DisplayName = r.Name ?? ""
        });

        return Result<IEnumerable<RoleDto>>.Success(roles);
    }
}

