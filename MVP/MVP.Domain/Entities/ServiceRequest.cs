using MVP.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MVP.Domain.Entities;

public class ServiceRequest
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public RequestStatus Status { get; set; } = RequestStatus.Pending;

    public string CustomerId { get; set; } = string.Empty;

    public string? ProviderId { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ApplicationUser Customer { get; set; } = default!;
    public ApplicationUser? Provider { get; set; }
}
