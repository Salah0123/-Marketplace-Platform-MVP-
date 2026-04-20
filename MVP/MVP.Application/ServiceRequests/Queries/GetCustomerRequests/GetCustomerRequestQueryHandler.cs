using MediatR;
using MVP.Domain.Common;
using MVP.Domain.Dtos;
using MVP.Services.IServices;

namespace MVP.Application.ServiceRequests.Queries.GetCustomerRequests;

public sealed class GetCustomerRequestQueryHandler(IServiceRequestService serviceRequestService) : IRequestHandler<GetCustomerRequestQuery, Result<IEnumerable<ServiceRequestResponse>>>
{
    private readonly IServiceRequestService _serviceRequestService = serviceRequestService;

    public async Task<Result<IEnumerable<ServiceRequestResponse>>> Handle(GetCustomerRequestQuery request, CancellationToken cancellationToken)
    {
        var result = await _serviceRequestService.GetCustomerRequestsAsync(request.CustomerId, cancellationToken);

        return result;
    }
}
