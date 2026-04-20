using MediatR;
using MVP.Domain.Common;
using MVP.Domain.Dtos;
using MVP.Services.IServices;

namespace MVP.Application.Auth.Queries.GetToken;

public class GetTokenHandler(IAuthService authService) : IRequestHandler<GetTokenQuery, Result<AuthResponse?>>
{
    private readonly IAuthService _authService = authService;
    public async Task<Result<AuthResponse?>> Handle(GetTokenQuery request, CancellationToken cancellationToken)
    {
        var result = await _authService.GetTokenAsync(request.Email, request.Password);
        return result;
    }
}
