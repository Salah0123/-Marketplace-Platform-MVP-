using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVP.Domain.Common;
using MVP.Domain.Dtos;
using MVP.Domain.Entities;
using MVP.Domain.Enums;
using MVP.Infrastructure.Data;
using MVP.Services.IServices;

namespace MVP.Services.Services;

public class ServiceRequestService(ApplicationDbContext context) : IServiceRequestService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<int>> CreateAsync(ServiceRequest serviceRequest, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Id == serviceRequest.CustomerId);

        if (user is null)
            return Result<int>.Failure(new Error("User not found", "User not found", StatusCodes.Status404NotFound));

        if (user.IsFreeSubscribtion == (int)SubscriptionType.Free)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            var requestCount = await _context.ServiceRequests
                .CountAsync(r => r.CustomerId == user.Id, cancellationToken);

            if (requestCount >= 3)
                return Result<int>.Failure(new Error(
                    "SubscriptionLimit",
                    "You have reached the free limit. Please subscribe.",
                    StatusCodes.Status400BadRequest));
            serviceRequest.CreatedAt = DateTime.UtcNow;
            var sa = 54;
            await _context.ServiceRequests.AddAsync(serviceRequest, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return Result<int>.Success(serviceRequest.Id);
        }


        serviceRequest.Status = RequestStatus.Pending;
        serviceRequest.CreatedAt = DateTime.UtcNow;
        await _context.ServiceRequests.AddAsync(serviceRequest, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(serviceRequest.Id);
    }

    public async Task<Result<IEnumerable<ServiceRequestResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var serviceRequests = await _context.ServiceRequests.Include(x=>x.Customer).Include(x=>x.Provider).ToListAsync(cancellationToken);
        var serviceRequestResponses = serviceRequests.Select(sr => new ServiceRequestResponse
        {
            Id = sr.Id,
            Title = sr.Title,
            Description = sr.Description,
            Status = sr.Status.ToString(),
            Latitude = sr.Latitude,
            Longitude = sr.Longitude,
            Customer = sr.Customer.Name,
            Provider = sr.Provider?.Name,
            CreatedAt = sr.CreatedAt

        }).ToList();
        return Result<IEnumerable<ServiceRequestResponse>>.Success(serviceRequestResponses);
    }



    public async Task<Result<IEnumerable<ServiceRequestResponse>>> GetCustomerRequestsAsync(string customerId,CancellationToken cancellationToken = default)
    {
        var serviceRequests = await _context.ServiceRequests.Include(x => x.Customer).Include(x => x.Provider)
            .Where(x=>x.CustomerId == customerId)
            .ToListAsync(cancellationToken);
        var serviceRequestResponses = serviceRequests.Select(sr => new ServiceRequestResponse
        {
            Id = sr.Id,
            Title = sr.Title,
            Description = sr.Description,
            Status = sr.Status.ToString(),
            Latitude = sr.Latitude,
            Longitude = sr.Longitude,
            Customer = sr.Customer.Name,
            Provider = sr.Provider?.Name,
            CreatedAt = sr.CreatedAt
        }).ToList();
        return Result<IEnumerable<ServiceRequestResponse>>.Success(serviceRequestResponses);
    }


    public async Task<Result<IEnumerable<ServiceRequestResponse>>> GetProviderRequestsAsync(string providerId,CancellationToken cancellationToken = default)
    {
        var serviceRequests = await _context.ServiceRequests.Include(x => x.Customer).Include(x => x.Provider)
            .Where(x=> x.ProviderId  == providerId)
            .ToListAsync(cancellationToken);
        var serviceRequestResponses = serviceRequests.Select(sr => new ServiceRequestResponse
        {
            Id = sr.Id,
            Title = sr.Title,
            Description = sr.Description,
            Status = sr.Status.ToString(),
            Latitude = sr.Latitude,
            Longitude = sr.Longitude,
            Customer = sr.Customer.Name,
            Provider = sr.Provider?.Name,
            CreatedAt = sr.CreatedAt
        }).ToList();
        return Result<IEnumerable<ServiceRequestResponse>>.Success(serviceRequestResponses);
    }


    public async Task<Result<IEnumerable<ServiceRequestResponse>>> GetNearbyAsync(
    double providerLat,
    double providerLng,
    CancellationToken cancellationToken = default)
    {
        var requests = await _context.ServiceRequests
            .Where(r => r.Status == RequestStatus.Pending)
            .Include(x => x.Customer)
            .Include(x => x.Provider)
            .ToListAsync();

        var nearby = requests
            .Where(r => GetDistance(providerLat, providerLng, r.Latitude, r.Longitude) <= 5)
            .ToList();


        var response = nearby.Select(sr => new ServiceRequestResponse
        {
            Id = sr.Id,
            Title = sr.Title,
            Description = sr.Description,
            Status = sr.Status.ToString(),
            Latitude = sr.Latitude,
            Longitude = sr.Longitude,
            Customer = sr.Customer.Name,
            Provider = sr.Provider?.Name
        }).ToList();
        return Result<IEnumerable<ServiceRequestResponse>>.Success(response);
    }

    

    public async Task<Result> AcceptAsync(int id, string providerId, CancellationToken cancellationToken = default)
    {
        var provider = await _context.Users.FirstOrDefaultAsync(p => p.Id == providerId, cancellationToken);
        if (provider is null)
            return Result.Failure(new Error("Provider not found.", "Provider not found.", StatusCodes.Status404NotFound));

        var serviceRequest = await _context.ServiceRequests.FirstOrDefaultAsync(x=>x.Id == id, cancellationToken);
        if (serviceRequest is null)
            return Result.Failure(new Error("Service request not found.", "Service request not found.", StatusCodes.Status404NotFound));
        serviceRequest.Status = RequestStatus.Accepted;
        serviceRequest.ProviderId = providerId;
        serviceRequest.UpdatedAt = DateTime.UtcNow;
        _context.ServiceRequests.Update(serviceRequest);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }



    public async Task<Result> CompleteAsync(int id, string providerId, CancellationToken cancellationToken = default)
    {
        var provider = await _context.Users.FirstOrDefaultAsync(p => p.Id == providerId, cancellationToken);
        if (provider is null)
            return Result.Failure(new Error("Provider not found.", "Provider not found.", StatusCodes.Status404NotFound));

        var serviceRequest = await _context.ServiceRequests.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (serviceRequest is null)
            return Result.Failure(new Error("Service request not found.", "Service request not found.", StatusCodes.Status404NotFound));

        if (serviceRequest.Status != RequestStatus.Accepted)
            return Result.Failure(new Error("Service is not accepted", "Service is not accepted", StatusCodes.Status400BadRequest));

        serviceRequest.Status = RequestStatus.Completed;
        serviceRequest.UpdatedAt = DateTime.UtcNow;
        _context.ServiceRequests.Update(serviceRequest);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }



    private double GetDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371; // نصف قطر الأرض بالكيلومتر

        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);

        var a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return R * c; // المسافة بالكيلومتر
    }


    private double ToRadians(double angle)
    {
        return angle * Math.PI / 180;
    }
}
