using MediatR;
using MVP.Application.ServiceRequests.Commands.AcceptRequest;
using MVP.Domain.Common;
using MVP.Services.IServices;

namespace MVP.Application.ServiceRequests.Commands.CompleteRequest;

public sealed class CompleteRequestCommandHandler(IServiceRequestService serviceRequestService) : IRequestHandler<CompleteRequestCommand, Result>
{
    private readonly IServiceRequestService _serviceRequestService = serviceRequestService;
    public async Task<Result> Handle(CompleteRequestCommand request, CancellationToken cancellationToken)
    {
        var result = await _serviceRequestService.CompleteAsync(request.Id, request.ProvuderId, cancellationToken);
        return result;
    }
}