using MediatR;
using MVP.Domain.Common;
using MVP.Domain.Dtos;
using MVP.Services.IServices;

namespace MVP.Application.ServiceRequests.Queries.GetNearby;


public sealed class GetNearbyHandler(IServiceRequestService serviceRequestService) : IRequestHandler<GetNearbyQuery, Result<IEnumerable<ServiceRequestResponse>>>
{
    private readonly IServiceRequestService _serviceRequestService = serviceRequestService;

    public async Task<Result<IEnumerable<ServiceRequestResponse>>> Handle(GetNearbyQuery request, CancellationToken cancellationToken)
    {
        var result = await _serviceRequestService.GetNearbyAsync(request.ProviderLat, request.ProviderLng, cancellationToken);

        return result;
    }
}
