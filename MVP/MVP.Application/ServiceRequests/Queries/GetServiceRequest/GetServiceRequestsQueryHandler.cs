using MediatR;
using MVP.Domain.Common;
using MVP.Domain.Dtos;
using MVP.Services.IServices;

namespace MVP.Application.ServiceRequests.Queries.GetServiceRequestById;

public sealed class GetServiceRequestsQueryHandler(IServiceRequestService serviceRequestService) : IRequestHandler<GetServiceRequestsQuery, Result<IEnumerable<ServiceRequestResponse>>>
{
    private readonly IServiceRequestService _serviceRequestService = serviceRequestService;

    public async Task<Result<IEnumerable<ServiceRequestResponse>>> Handle(GetServiceRequestsQuery request, CancellationToken cancellationToken)
    {
        var result =await _serviceRequestService.GetAllAsync(cancellationToken);

        return result;
    }
}
