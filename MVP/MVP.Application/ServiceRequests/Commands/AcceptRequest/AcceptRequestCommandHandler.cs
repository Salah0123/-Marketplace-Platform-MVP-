using MediatR;
using MVP.Domain.Common;
using MVP.Services.IServices;

namespace MVP.Application.ServiceRequests.Commands.AcceptRequest;


public sealed class AcceptRequestCommandHandler(IServiceRequestService serviceRequestService) : IRequestHandler<AcceptRequestCommand, Result>
{
    private readonly IServiceRequestService _serviceRequestService = serviceRequestService;
    public async Task<Result> Handle(AcceptRequestCommand request, CancellationToken cancellationToken)
    {
        var result = await _serviceRequestService.AcceptAsync(request.Id, request.ProvuderId, cancellationToken);
        return result;
    }
}