using MVP.Domain.Common;
using MVP.Domain.Dtos;

namespace MVP.Services.IServices;

public interface IAuthService
{
    Task<Result<AuthResponse?>> GetTokenAsync(string email, string password);
    Task<Result> RegisterAsync(string email, string password, string name, string createdBy);

    Task<Result<IEnumerable<UserResponse>>> GetUsersAsync();
    Task<Result<IEnumerable<string>>> GetRolesAsync();
    Task<Result> AddRoleAsync(string role, string roleKey, string DisplayName);

    Task<Result> AddUserToRole(string id, string[] roles);
    Task<Result> RemoveUserFromRole(string id, string[] roles);
    Task<Result> Subscription(string id);
}
