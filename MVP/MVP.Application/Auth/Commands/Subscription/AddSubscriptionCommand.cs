using MediatR;
using MVP.Domain.Common;

namespace MVP.Application.Auth.Commands.Subscription;

public record AddSubscriptionCommand(string Id) : IRequest<Result>;
