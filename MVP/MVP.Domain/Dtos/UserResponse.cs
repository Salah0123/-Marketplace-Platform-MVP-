namespace MVP.Domain.Dtos;

public class UserResponse
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public bool IsActive { get; set; }
    public bool IsFreeSubscription { get; set; }
    public string? DisplayName { get; set; }
    public string? Email { get; set; }
    public IEnumerable<string> Roles { get; set; } = [];
}
