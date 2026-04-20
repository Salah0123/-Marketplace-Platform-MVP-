using MediatR;
using MVP.Domain.Common;

namespace MVP.Application.ServiceRequests.Commands.CompleteRequest;


public sealed record CompleteRequestCommand(int Id, string ProvuderId) : IRequest<Result>;