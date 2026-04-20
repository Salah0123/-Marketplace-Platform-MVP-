using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVP.Api.Abstractions;
using MVP.Api.Extensions;
using MVP.Application.Auth.Commands.RegisterUser;
using MVP.Application.Auth.Commands.Subscription;
using MVP.Application.Auth.Queries.GetRoles;
using MVP.Application.Auth.Queries.GetToken;
using MVP.Application.Auth.Queries.GetUsers;
using MVP.Application.Roles.Commands.AddRole;
using MVP.Application.Roles.Commands.AddUserToRoles;
using MVP.Application.Roles.Commands.RemoveUserFromRoles;
using MVP.Domain.Shared;

namespace MVP.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] GetTokenQuery command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem(StatusCodes.Status400BadRequest);
    }
    

    [HttpPost("register")]
    [Authorize(Roles = SystemRoles.Admin)]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var updatedCardRequest = command with
        {
            CreatedBy = User.GetUserId()
        };
        var result = await _mediator.Send(updatedCardRequest);
        return result.IsSuccess ? Ok() : result.ToProblem(StatusCodes.Status400BadRequest);
    }

    [HttpGet("")]
    [Authorize(Roles = SystemRoles.Admin)]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _mediator.Send(new GetUsersQuery());
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem(StatusCodes.Status400BadRequest);
    }

    [HttpGet("roles")]
    [Authorize(Roles = SystemRoles.Admin)]
    public async Task<IActionResult> GetRoles()
    {
        var result = await _mediator.Send(new GetRolesQuery());
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem(StatusCodes.Status400BadRequest);
    }
    [HttpPost("roles")]
    [Authorize(Roles = SystemRoles.Admin)]
    public async Task<IActionResult> AddRole([FromBody] AddRoleCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok() : result.ToProblem(StatusCodes.Status400BadRequest);
    }

    [HttpPost("{id}/roles")]
    [Authorize(Roles = SystemRoles.Admin)]
    public async Task<IActionResult> AddUserToRoles(string id, [FromBody] string[] roles)
    {
        var result = await _mediator.Send(new AddUserToRolesCommand(id, roles));
        return result.IsSuccess ? Ok() : result.ToProblem(StatusCodes.Status400BadRequest);
    }

    [HttpDelete("{id}/roles")]
    [Authorize(Roles = SystemRoles.Admin)]
    public async Task<IActionResult> RemoveUserFromRoles(string id, [FromBody] string[] roles)
    {
        var result = await _mediator.Send(new RemoveUserFromRolesCommand(id, roles));
        return result.IsSuccess ? Ok() : result.ToProblem(StatusCodes.Status400BadRequest);
    }

    [HttpPost("add-subscription")]
    [Authorize(Roles = SystemRoles.Customer)]
    public async Task<IActionResult> AddSubscription()
    {
        var result = await _mediator.Send(new AddSubscriptionCommand(User.GetUserId()!));
        return result.IsSuccess ? Ok() : result.ToProblem((int)result.Error.StatusCode!);
    }
}
