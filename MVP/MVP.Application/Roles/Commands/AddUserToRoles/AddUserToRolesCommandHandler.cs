using MediatR;
using MVP.Domain.Common;
using MVP.Services.IServices;

namespace MVP.Application.Roles.Commands.AddUserToRoles;

public class AddUserToRolesCommandHandler(IAuthService authService) : IRequestHandler<AddUserToRolesCommand, Result>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result> Handle(AddUserToRolesCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.AddUserToRole(request.UserId, request.Roles);
        if (!result.IsSuccess)
            return Result.Failure(result.Error);
        return Result.Success();
    }
}