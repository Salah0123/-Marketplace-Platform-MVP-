using Microsoft.AspNetCore.Identity;
using MVP.Domain.Entities;
using MVP.Domain.Enums;
using MVP.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Infrastructure.SeedingData;

public static class UserSeeder
{
    public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
    {
        var adminEmail = "hossameldinsalah67@gmail.com";
        var adminUserName = "1446112";

        var existingUser = await userManager.FindByEmailAsync(adminEmail);
        if (existingUser != null)
            return; // موجود مسبقًا، لا نفعل أي شيء

        var user = new ApplicationUser
        {
            UserName = adminUserName,   
            Email = adminEmail,
            Name = "حسام الدين صلاح مصطفى",
            IsActive = (int)ActiveStatus.Active,
            CreatedAt = DateTime.Now
        };

        // إنشاء المستخدم بكلمة سر
        var password = "@1Password"; // تغيير كلمة المرور الافتراضية حسب الحاجة
        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        // إضافة المستخدم إلى Roles
        await userManager.AddToRolesAsync(user, SystemRoles.All); // هنا نضيفه لكل الـ Roles
    }
}
