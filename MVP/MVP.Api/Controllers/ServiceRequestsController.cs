using MediatR;
using Microsoft.AspNetCore.Mvc;
using MVP.Api.Abstractions;
using MVP.Application.ServiceRequests.Commands.CreateServiceRequest;
using MVP.Application.ServiceRequests.Queries.GetServiceRequestById;
using MVP.Application.ServiceRequests.Queries.GetNearby;
using MVP.Application.ServiceRequests.Commands.AcceptRequest;
using MVP.Application.ServiceRequests.Commands.CompleteRequest;
using MVP.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using MVP.Domain.Shared;
using MVP.Application.ServiceRequests.Queries.GetCustomerRequests;
using MVP.Application.ServiceRequests.Queries.GetProviderRequests;

namespace MVP.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ServiceRequestsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    [HttpGet("")]
    [Authorize(Roles = SystemRoles.Admin)]
    public async Task<IActionResult> GetServiceRequests()
    {
        var result = await _mediator.Send(new GetServiceRequestsQuery());
        return Ok(result.Value);
    }


    [HttpGet("customer")]
    [Authorize(Roles = SystemRoles.Customer)]
    public async Task<IActionResult> GetCustomerServiceRequests()
    {
        var result = await _mediator.Send(new GetCustomerRequestQuery(User.GetUserId()!));
        return Ok(result.Value);
    }


    [HttpGet("provider")]
    [Authorize(Roles = SystemRoles.Provider)]
    public async Task<IActionResult> GetProviderServiceRequests()
    {
        var result = await _mediator.Send(new GetProviderRequestsQuery(User.GetUserId()!));
        return Ok(result.Value);
    }

    //[HttpGet("id")]
    //public async Task<IActionResult> GetServiceRequestById(int id)
    //{
    //    var result = await _mediator.Send(new GetServiceRequestByIdQuery(id));
    //    return result is null ? NotFound() : Ok(result);
    //}

    [HttpPost("")]
    [Authorize(Roles = SystemRoles.Customer)]
    public async Task<IActionResult> CreateServiceRequest(CreateServiceRequestCommand request)
    {
        var command = request with { UserId = User.GetUserId()! };
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Created() : result.ToProblem(result.Error.StatusCode ?? StatusCodes.Status400BadRequest);
    }

    [HttpGet("nearby")]
    public async Task<IActionResult> GetNearby([FromQuery] double providerLat, [FromQuery] double providerLng)
    {
        var result = await _mediator.Send(new GetNearbyQuery(providerLat, providerLng));
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem(result.Error.StatusCode ?? StatusCodes.Status400BadRequest);
    }

    [HttpPost("{id}/accept")]
    [Authorize(Roles = SystemRoles.Provider)]
    public async Task<IActionResult> AcceptRequest(int id)
    {
        var result = await _mediator.Send(new AcceptRequestCommand(id, User.GetUserId()!));
        return result.IsSuccess ? Ok() : result.ToProblem(result.Error.StatusCode ?? StatusCodes.Status400BadRequest);
    }

    [HttpPost("{id}/complete")]
    [Authorize(Roles = SystemRoles.Provider)]
    public async Task<IActionResult> CompleteRequest(int id)
    {
        var result = await _mediator.Send(new CompleteRequestCommand(id, User.GetUserId()!));
        return result.IsSuccess ? Ok() : result.ToProblem(result.Error.StatusCode ?? StatusCodes.Status400BadRequest);
    }
}
