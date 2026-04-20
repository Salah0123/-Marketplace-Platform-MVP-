using Microsoft.AspNetCore.Identity;
using MVP.Domain.Entities;
using MVP.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVP.Infrastructure.SeedingData;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
    {
        foreach (var roleKey in SystemRoles.All)
        {
            if (await roleManager.RoleExistsAsync(roleKey))
                continue;

            var role = new ApplicationRole
            {
                Name = roleKey,
            };

            await roleManager.CreateAsync(role);
        }

    }
}
