using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using MVP.Domain.Entities;
using MVP.Domain.Enums;
using MVP.Infrastructure.Data;
using MVP.Services.Services;
using System.Linq.Expressions;
using Xunit;

namespace MVP.Services.Tests;

public class ServiceRequestServiceTests
{
    private readonly Mock<ApplicationDbContext> _contextMock;
    private readonly ServiceRequestService _service;

    public ServiceRequestServiceTests()
    {
        _contextMock = new Mock<ApplicationDbContext>();   // Loose mode (الأسهل للمبتدئين)
        _service = new ServiceRequestService(_contextMock.Object);
    }

    // ==================== CreateAsync Tests ====================

    [Fact]
    public async Task CreateAsync_Should_Add_And_Save_And_Return_Success()
    {
        // Arrange
        var serviceRequest = new ServiceRequest
        {
            Title = "Fix my sink",
            Description = "The kitchen sink is leaking",
            Latitude = 30.0444,
            Longitude = 31.2357,
            CustomerId = "customer-123",
            Status = RequestStatus.Pending
        };

        var mockDbSet = new Mock<DbSet<ServiceRequest>>();

        _contextMock.Setup(c => c.ServiceRequests).Returns(mockDbSet.Object);
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(1);

        // Act
        var result = await _service.CreateAsync(serviceRequest, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeGreaterThan(0);

        mockDbSet.Verify(m => m.AddAsync(It.IsAny<ServiceRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    // ==================== AcceptAsync Tests ====================

    [Fact]
    public async Task AcceptAsync_Should_Return_Failure_When_Provider_Not_Found()
    {
        _contextMock.Setup(c => c.Users.FirstOrDefaultAsync(
            It.IsAny<Expression<Func<ApplicationUser, bool>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((ApplicationUser?)null);

        var result = await _service.AcceptAsync(100, "provider-not-exist");

        result.IsSuccess.Should().BeFalse();
        result.Error?.Description.Should().Contain("Provider not found");
    }

    [Fact]
    public async Task AcceptAsync_Should_Update_Status_When_Valid()
    {
        var provider = new ApplicationUser { Id = "provider-456" };
        var request = new ServiceRequest { Id = 50, Status = RequestStatus.Pending };

        _contextMock.Setup(c => c.Users.FirstOrDefaultAsync(
            It.IsAny<Expression<Func<ApplicationUser, bool>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        _contextMock.Setup(c => c.ServiceRequests.FirstOrDefaultAsync(
            It.IsAny<Expression<Func<ServiceRequest, bool>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(request);

        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(1);

        var result = await _service.AcceptAsync(50, "provider-456");

        result.IsSuccess.Should().BeTrue();
        request.Status.Should().Be(RequestStatus.Accepted);
        request.ProviderId.Should().Be("provider-456");
    }

    // ==================== CompleteAsync Tests ====================

    [Fact]
    public async Task CompleteAsync_Should_Return_Failure_When_Not_Accepted()
    {
        var request = new ServiceRequest { Id = 60, Status = RequestStatus.Pending };

        _contextMock.Setup(c => c.Users.FirstOrDefaultAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new ApplicationUser());

        _contextMock.Setup(c => c.ServiceRequests.FirstOrDefaultAsync(It.IsAny<Expression<Func<ServiceRequest, bool>>>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(request);

        var result = await _service.CompleteAsync(60, "p1");

        result.IsSuccess.Should().BeFalse();
    }
}