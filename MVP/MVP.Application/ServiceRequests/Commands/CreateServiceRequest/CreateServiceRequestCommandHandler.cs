using MediatR;
using MVP.Domain.Common;
using MVP.Domain.Entities;
using MVP.Services.IServices;

namespace MVP.Application.ServiceRequests.Commands.CreateServiceRequest;

public sealed class CreateServiceRequestCommandHandler(IServiceRequestService serviceRequestService) : IRequestHandler<CreateServiceRequestCommand, Result>
{
    private readonly IServiceRequestService _serviceRequestService = serviceRequestService;
    public async Task<Result> Handle(CreateServiceRequestCommand request, CancellationToken cancellationToken)
    {
        var result = await _serviceRequestService.CreateAsync(new ServiceRequest
        {
            Title = request.Title,
            Description = request.Description,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            CustomerId = request.UserId!,
        });
        return result;
    }
}
