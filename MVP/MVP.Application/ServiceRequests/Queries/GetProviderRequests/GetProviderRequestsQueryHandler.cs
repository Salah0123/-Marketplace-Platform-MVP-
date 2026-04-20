using MediatR;
using MVP.Domain.Common;
using MVP.Domain.Dtos;
using MVP.Services.IServices;

namespace MVP.Application.ServiceRequests.Queries.GetProviderRequests;

public sealed class GetProviderRequestsQueryHandler(IServiceRequestService serviceRequestService) : IRequestHandler<GetProviderRequestsQuery, Result<IEnumerable<ServiceRequestResponse>>>
{
    private readonly IServiceRequestService _serviceRequestService = serviceRequestService;

    public async Task<Result<IEnumerable<ServiceRequestResponse>>> Handle(GetProviderRequestsQuery request, CancellationToken cancellationToken)
    {
        var result = await _serviceRequestService.GetProviderRequestsAsync(request.CustomerId, cancellationToken);

        return result;
    }
}