using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using MVP.Api.Extensions;
using MVP.Application;
using MVP.Domain.Entities;
using MVP.Infrastructure;
using MVP.Infrastructure.Data;
using MVP.Infrastructure.SeedingData;
using MVP.Services;
using Serilog;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddInfrastructureDependencies(builder.Configuration)
    .AddServiceDependencies(builder.Configuration)
    .AddApplicationDependencies();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddCors(option =>
{
    option.AddPolicy("DefaultOrigin",
         builder => builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod());
});


builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


builder.Services.AddRateLimiter(options =>
{
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429; // Service Unavailable
        await context.HttpContext.Response.WriteAsync("تم رفض الطلب: النظام مشغول حالياً.", cancellationToken: token);
    };
    options.AddConcurrencyLimiter("concurrency", opt =>
    {
        opt.PermitLimit = 1000; // Maximum number of concurrent requests
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 100; // Maximum number of requests in the queue
    });
});

//builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // أولاً Seed Roles
    await RoleSeeder.SeedRolesAsync(roleManager);

    // بعد ذلك Seed User و Assign Roles
    await UserSeeder.SeedUserAsync(userManager);

}



app.UseExceptionHandler();
app.UseSerilogRequestLogging();
app.UseCors("DefaultOrigin");
app.UseRateLimiter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
