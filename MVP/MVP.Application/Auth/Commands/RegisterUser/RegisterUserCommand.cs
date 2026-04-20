using MediatR;
using MVP.Domain.Common;

namespace MVP.Application.Auth.Commands.RegisterUser;

public record RegisterUserCommand(string Email, string Password, string Name, string? CreatedBy) : IRequest<Result>;



