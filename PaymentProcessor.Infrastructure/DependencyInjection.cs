using Microsoft.Extensions.DependencyInjection;
using PaymentProcessor.Core.Interfaces;
using PaymentProcessor.Infrastructure.Repositories;

namespace PaymentProcessor.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        return services;
    }
}