using MediatR;
using MVP.Domain.Common;
using MVP.Domain.Dtos;

namespace MVP.Application.Auth.Queries.GetUsers;

public record GetUsersQuery() : IRequest<Result<IEnumerable<UserResponse>>>;
