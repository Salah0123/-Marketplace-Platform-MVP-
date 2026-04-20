using MediatR;
using MVP.Domain.Common;
using MVP.Services.IServices;

namespace MVP.Application.Auth.Commands.RegisterUser;

public class RegisterUserCommandHandler(IAuthService authService) : IRequestHandler<RegisterUserCommand, Result>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request.Email, request.Password, request.Name, request.CreatedBy!);
        return result;
    }
}
