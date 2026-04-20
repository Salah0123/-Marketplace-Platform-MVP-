using MediatR;
using MVP.Domain.Common;
using MVP.Domain.Dtos;

namespace MVP.Application.ServiceRequests.Queries.GetNearby;

public sealed record GetNearbyQuery(double ProviderLat, double ProviderLng) : IRequest<Result<IEnumerable<ServiceRequestResponse>>>;
