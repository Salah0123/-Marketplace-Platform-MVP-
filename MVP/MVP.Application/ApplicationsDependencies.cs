using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MVP.Application.Common.Interfaces;
using System.Reflection;

namespace MVP.Application;

public static class ApplicationsDependencies
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Behaviors.ValidationBehavior<,>));

        //services.AddScoped<IApplicationDbContext, IApplicationDbContext>();

        return services;
    }
}

