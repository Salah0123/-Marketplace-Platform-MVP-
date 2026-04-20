using MediatR;
using MVP.Domain.Common;
using MVP.Domain.Dtos;
using MVP.Domain.Entities;

namespace MVP.Application.ServiceRequests.Queries.GetServiceRequestById;

public sealed record GetServiceRequestsQuery : IRequest<Result<IEnumerable<ServiceRequestResponse>>>;
