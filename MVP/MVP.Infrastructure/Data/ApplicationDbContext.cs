using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVP.Domain.Entities;
using MVP.Infrastructure.Data.Configurations;

namespace MVP.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)//, IApplicationDbContext
{
    public DbSet<ServiceRequest> ServiceRequests { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ServiceRequestConfiguration).Assembly);
    }
}
