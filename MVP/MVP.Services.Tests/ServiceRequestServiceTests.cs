using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MVP.Domain.Entities;
using MVP.Domain.Enums;
using MVP.Infrastructure.Data;
using MVP.Services.Services;
using Xunit;

namespace MVP.Services.Tests;

public class ServiceRequestServiceTests
{
    private ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
            .Options;

        return new ApplicationDbContext(options);
    }

    // ==================== CreateAsync Tests ====================

    [Fact]
    public async Task CreateAsync_Should_Fail_When_Limit_Reached()
    {
        var context = CreateDbContext();

        var customerId = "customer-123";

        context.Users.Add(new ApplicationUser
        {
            Id = customerId,
            IsFreeSubscribtion = (int)SubscriptionType.Free
        });

        context.ServiceRequests.AddRange(
            new ServiceRequest { CustomerId = customerId },
            new ServiceRequest { CustomerId = customerId },
            new ServiceRequest { CustomerId = customerId }
        );

        await context.SaveChangesAsync();

        var service = new ServiceRequestService(context);

        var request = new ServiceRequest
        {
            CustomerId = customerId,
            Title = "New Request"
        };

        var result = await service.CreateAsync(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error!.Code.Should().Be("SubscriptionLimit");
    }

    // ==================== AcceptAsync Tests ====================

    [Fact]
    public async Task AcceptAsync_Should_Return_Failure_When_Provider_Not_Found()
    {
        // Arrange
        var context = CreateDbContext();
        var service = new ServiceRequestService(context);

        var request = new ServiceRequest
        {
            Id = 100,
            Status = RequestStatus.Pending
        };

        context.ServiceRequests.Add(request);
        await context.SaveChangesAsync();

        // Act
        var result = await service.AcceptAsync(100, "provider-not-exist");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error?.Description.Should().Contain("Provider not found");
    }

    [Fact]
    public async Task AcceptAsync_Should_Update_Status_When_Valid()
    {
        // Arrange
        var context = CreateDbContext();
        var service = new ServiceRequestService(context);

        var provider = new ApplicationUser
        {
            Id = "provider-456"
        };

        var request = new ServiceRequest
        {
            Id = 50,
            Status = RequestStatus.Pending
        };

        context.Users.Add(provider);
        context.ServiceRequests.Add(request);
        await context.SaveChangesAsync();

        // Act
        var result = await service.AcceptAsync(50, "provider-456");

        // Assert
        result.IsSuccess.Should().BeTrue();
        request.Status.Should().Be(RequestStatus.Accepted);
        request.ProviderId.Should().Be("provider-456");
    }

    // ==================== CompleteAsync Tests ====================

    [Fact]
    public async Task CompleteAsync_Should_Return_Failure_When_Not_Accepted()
    {
        // Arrange
        var context = CreateDbContext();
        var service = new ServiceRequestService(context);

        var provider = new ApplicationUser
        {
            Id = "p1"
        };

        var request = new ServiceRequest
        {
            Id = 60,
            Status = RequestStatus.Pending
        };

        context.Users.Add(provider);
        context.ServiceRequests.Add(request);
        await context.SaveChangesAsync();

        // Act
        var result = await service.CompleteAsync(60, "p1");

        // Assert
        result.IsSuccess.Should().BeFalse();
    }
}