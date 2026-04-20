using MediatR;
using MVP.Domain.Common;

namespace MVP.Application.ServiceRequests.Commands.AcceptRequest;

public sealed record AcceptRequestCommand(int Id, string ProvuderId) : IRequest<Result>;