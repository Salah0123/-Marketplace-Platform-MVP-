using MediatR;
using MVP.Domain.Common;
using MVP.Domain.Dtos;

namespace MVP.Application.ServiceRequests.Queries.GetProviderRequests;

public sealed record GetProviderRequestsQuery(string CustomerId) : IRequest<Result<IEnumerable<ServiceRequestResponse>>>;