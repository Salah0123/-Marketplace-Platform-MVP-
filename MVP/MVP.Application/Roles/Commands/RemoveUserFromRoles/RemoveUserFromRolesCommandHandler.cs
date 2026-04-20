using MediatR;
using MVP.Domain.Common;
using MVP.Services.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVP.Application.Roles.Commands.RemoveUserFromRoles;

public class RemoveUserFromRolesCommandHandler(IAuthService authService) : IRequestHandler<RemoveUserFromRolesCommand, Result>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result> Handle(RemoveUserFromRolesCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.RemoveUserFromRole(request.UserId, request.Roles);
        if (!result.IsSuccess)
            return Result.Failure(result.Error);
        return Result.Success();
    }
}
