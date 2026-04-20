using Microsoft.EntityFrameworkCore;
using MVP.Domain.Entities;

namespace MVP.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ServiceRequest> ServiceRequests { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
