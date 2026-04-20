using Microsoft.AspNetCore.Identity;
using MVP.Domain.Enums;

namespace MVP.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;
    public int IsActive { get; set; } = (int)ActiveStatus.Active;
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
    public int IsFreeSubscribtion { get; set; } = (int)SubscriptionType.Free;
}
