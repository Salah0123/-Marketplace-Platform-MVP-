using MediatR;
using MVP.Domain.Common;
using MVP.Services.IServices;

namespace MVP.Application.Auth.Commands.Subscription;

public class AddSubscriptionCommandHandler(IAuthService authService) : IRequestHandler<AddSubscriptionCommand, Result>
{
    private readonly IAuthService _authService = authService;

    public async Task<Result> Handle(AddSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.Subscription(request.Id);
        return result;
    }
}