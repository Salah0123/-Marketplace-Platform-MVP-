using MediatR;
using MVP.Domain.Common;
using MVP.Domain.Dtos;

namespace MVP.Application.ServiceRequests.Queries.GetCustomerRequests;

public sealed record GetCustomerRequestQuery(string CustomerId) : IRequest<Result<IEnumerable<ServiceRequestResponse>>>;