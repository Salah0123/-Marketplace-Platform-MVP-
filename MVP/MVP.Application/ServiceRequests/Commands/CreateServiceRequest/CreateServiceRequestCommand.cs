using MediatR;
using MVP.Domain.Common;

namespace MVP.Application.ServiceRequests.Commands.CreateServiceRequest;

public sealed record CreateServiceRequestCommand(
    string Title,
    string Description,
    double Latitude,
    double Longitude,
    string? UserId
) : IRequest<Result>;

