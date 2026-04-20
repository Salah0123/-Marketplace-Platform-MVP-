using MediatR;
using MVP.Domain.Common;
using MVP.Services.IServices;

namespace MVP.Application.Roles.Commands.AddRole;

public class AddRoleCommandHandler(IAuthService authService) : IRequestHandler<AddRoleCommand, Result>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.AddRoleAsync(request.Role, request.RoleKey, request.DisplayName);
        if (!result.IsSuccess)
            return Result.Failure(result.Error);
        return Result.Success();
    }
}