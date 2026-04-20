using MVP.Domain.Enums;

namespace MVP.Domain.Dtos;

public class ServiceRequestResponse
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Customer { get; set; }
    public string? Provider { get; set; }
}
