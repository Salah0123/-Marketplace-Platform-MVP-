namespace MVP.Services.IServices;

public interface IJwtService
{
    (string token, int expiresIn) GenerateToken(string username, string? id, string email, string name, bool isActive, bool isFreeSubscribtion, IEnumerable<string>? roles);
}
