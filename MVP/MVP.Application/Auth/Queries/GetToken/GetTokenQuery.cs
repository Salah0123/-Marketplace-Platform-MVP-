using MediatR;
using MVP.Domain.Common;
using MVP.Domain.Dtos;

namespace MVP.Application.Auth.Queries.GetToken;

public record GetTokenQuery(string Email, string Password) : IRequest<Result<AuthResponse?>>;

