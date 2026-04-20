using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MVP.Api.Extensions;

public static class UserExtensions
{
    public static string? GetUserId(this ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.NameIdentifier);


    public static string? GetUsername(this ClaimsPrincipal user) =>
        user.FindFirstValue(JwtRegisteredClaimNames.PreferredUsername);

    public static string? GetEmail(this ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.Email);

    public static IList<string> GetRoles(this ClaimsPrincipal user) =>
        user.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
}
