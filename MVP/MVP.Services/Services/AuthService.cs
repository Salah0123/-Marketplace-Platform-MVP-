using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVP.Domain.Common;
using MVP.Domain.Dtos;
using MVP.Domain.Entities;
using MVP.Domain.Enums;
using MVP.Infrastructure.Data;
using MVP.Services.IServices;

namespace MVP.Services.Services;

public class AuthService(
        IJwtService jwtService,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager, ApplicationDbContext context) : IAuthService
{
    private readonly IJwtService _jwtService = jwtService;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<AuthResponse?>> GetTokenAsync(string email, string password)
    {
        
        var user = _userManager.Users.FirstOrDefault(u => u.Email == email);
        if (user is null)
            return Result<AuthResponse?>.Failure(new Error("user not found", "user not found", StatusCodes.Status404NotFound));
         var passwordValid = await _userManager.CheckPasswordAsync(user, password);
        if(!passwordValid)
            return Result<AuthResponse?>.Failure(new Error("invalid password", "invalid password", StatusCodes.Status400BadRequest));
        if ((ActiveStatus)user.IsActive != ActiveStatus.Active)
            return Result<AuthResponse?>.Failure(new Error("user not active", "user not active", StatusCodes.Status400BadRequest));

        var roles = _userManager.GetRolesAsync(user).Result;
        var isFreeAccount = user.IsFreeSubscribtion == (int)SubscriptionType.Free;
        var (token, expiresIn) = _jwtService.GenerateToken(user.UserName ?? "", user.Id, user.Email ?? "", user.Name, user.IsActive == (int)ActiveStatus.Active, isFreeAccount, roles);

        var response = new AuthResponse
        {
            Username = email,
            Token = token,
            ExpiresIn = expiresIn
        };
        return Result<AuthResponse?>.Success(response);
    }

    public async Task<Result> RegisterAsync(string email, string password, string name, string createdBy)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser is not null)
            return Result<AuthResponse?>.Failure(new Error("user already exists", "user already exists", StatusCodes.Status400BadRequest));


        var user = new ApplicationUser { Id = Guid.CreateVersion7().ToString(), UserName = email, CreatedBy = createdBy, Name = name, Email = email, IsActive = (int)ActiveStatus.Active };
        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
            return Result.Success();
        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return Result.Failure(new Error(errors, errors, StatusCodes.Status400BadRequest));
    }




    public async Task<Result<IEnumerable<UserResponse>>> GetUsersAsync()
    {
        var users = await _userManager.Users.OrderByDescending(x => x.CreatedAt).ToListAsync();
        var userResponses = new List<UserResponse>();
        foreach (var user in users)
        {
            var roleKeys = await _userManager.GetRolesAsync(user);

            var displayRoles = _roleManager.Roles
                .Where(r => r.Name != null && roleKeys.Contains(r.Name))
                .Select(r => r.Name!)
                .ToList();

            var userResponse = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                DisplayName = user.Name,
                IsFreeSubscription = user.IsFreeSubscribtion == (int)SubscriptionType.Free,
                IsActive = (ActiveStatus)user.IsActive == ActiveStatus.Active,
                Roles = displayRoles //await _userManager.GetRolesAsync(user)
            };
            userResponses.Add(userResponse);
        }
        return Result<IEnumerable<UserResponse>>.Success(userResponses);
    }



    public async Task<Result<IEnumerable<string>>> GetRolesAsync()
    {
        var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        return Result<IEnumerable<string>>.Success(roles.Where(role => role is not null).Select(role => role!));
    }
    public async Task<Result> AddRoleAsync(string role, string RoleKey, string DisplayName)
    {
        var newRole = new ApplicationRole
        {
            Name = role,
            NormalizedName = role.ToUpperInvariant(),
        };
        var result = await _roleManager.CreateAsync(newRole);
        if (result.Succeeded)
            return Result.Success();
        return Result<IEnumerable<string>>.Failure(new Error(string.Join(", ", result.Errors.Select(e => e.Description)), string.Join(", ", result.Errors.Select(e => e.Description)), StatusCodes.Status400BadRequest));
    }




    public async Task<Result> AddUserToRole(string id, string[] roles)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return Result.Failure(new Error("user not found", "user not found", StatusCodes.Status400BadRequest));

        // Validate all roles
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
                return Result.Failure(new Error($"Role not found: {role}", $"Role not found: {role}", StatusCodes.Status400BadRequest));
        }


        var currentRoles = await _userManager.GetRolesAsync(user);

        if(currentRoles.Count > 0)
        {
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeResult.Succeeded)
            {
                var removeResultErrors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                return Result.Failure(new Error(removeResultErrors, removeResultErrors, StatusCodes.Status400BadRequest));
            }
        }

        

        var result = await _userManager.AddToRolesAsync(user, roles);
        if (result.Succeeded) return Result.Success();
        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return Result.Failure(new Error(errors, errors, StatusCodes.Status400BadRequest));
    }

    public async Task<Result> RemoveUserFromRole(string id, string[] roles)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return Result.Failure(new Error("user not found", "user not found", StatusCodes.Status400BadRequest));

        // Validate all roles
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
                return Result.Failure(new Error($"Role not found: {role}", $"Role not found: {role}", StatusCodes.Status400BadRequest));
        }

        var result = await _userManager.RemoveFromRolesAsync(user, roles);
        if (result.Succeeded) return Result.Success();
        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return Result.Failure(new Error(errors, errors, StatusCodes.Status400BadRequest));
    }


    public async Task<Result> Subscription(string id)
    {
        var user =await _userManager.FindByIdAsync(id);
        if (user is null)
            return Result.Failure(new Error("user not found", "user not found", StatusCodes.Status400BadRequest));

        user.IsFreeSubscribtion = (int)SubscriptionType.Paid;
        await _userManager.UpdateAsync(user);
        return Result.Success();
    }

}
