using MediatR;
using MVP.Domain.Common;
using MVP.Domain.Dtos;
using MVP.Services.IServices;

namespace MVP.Application.Auth.Queries.GetUsers;

public class GetUsersQueryHandler(IAuthService authService) : IRequestHandler<GetUsersQuery, Result<IEnumerable<UserResponse>>>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result<IEnumerable<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await _authService.GetUsersAsync();
        return result.IsSuccess ? Result<IEnumerable<UserResponse>>.Success(result.Value) : Result<IEnumerable<UserResponse>>.Failure(result.Error);
    }
}