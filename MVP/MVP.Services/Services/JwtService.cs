using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MVP.Domain.Consts;
using MVP.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace MVP.Services.Services;

public class JwtService(IOptions<JwtOptions> jwtOption) : IJwtService
{
    private readonly JwtOptions _jwtOption = jwtOption.Value;
    public (string token, int expiresIn) GenerateToken(string username, string? id, string email, string name, bool isActive, bool isFreeSubscribtion, IEnumerable<string>? roles)
    {
        roles ??= [];

        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.PreferredUsername, username),
                new Claim(JwtRegisteredClaimNames.Name, name),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim("is_active", isActive.ToString().ToLower()),
                new Claim("is_free_subscribtion", isFreeSubscribtion.ToString().ToLower()),
                new Claim(nameof(roles), JsonSerializer.Serialize(roles), JsonClaimValueTypes.JsonArray),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

        if (!string.IsNullOrWhiteSpace(id))
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, id));
        }

        _ = _jwtOption.Key;

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Key));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var expirationDate = DateTime.UtcNow.AddMinutes(_jwtOption.ExpiryMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtOption.Issuer,
            audience: _jwtOption.Audience,
            claims: claims,
            expires: expirationDate,
            signingCredentials: signingCredentials
        );

        return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: _jwtOption.ExpiryMinutes * 60);
    }
}
