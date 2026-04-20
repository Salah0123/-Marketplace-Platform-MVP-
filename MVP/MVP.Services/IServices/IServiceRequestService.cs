using MVP.Domain.Common;
using MVP.Domain.Dtos;

namespace MVP.Services.IServices;

public interface IServiceRequestService
{
    Task<Result<int>> CreateAsync(MVP.Domain.Entities.ServiceRequest serviceRequest, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ServiceRequestResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ServiceRequestResponse>>> GetCustomerRequestsAsync(string customerId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ServiceRequestResponse>>> GetProviderRequestsAsync(string providerId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ServiceRequestResponse>>> GetNearbyAsync(double providerLat, double providerLng, CancellationToken cancellationToken = default);
    Task<Result> AcceptAsync(int id, string providerId, CancellationToken cancellationToken = default);
    Task<Result> CompleteAsync(int id, string providerId, CancellationToken cancellationToken = default);
}
