using Microsoft.Extensions.DependencyInjection;
using PaymentProcessor.Core.Interfaces;
using PaymentProcessor.Infrastructure.Repositories;

namespace PaymentProcessor.Infrastructure;

// Encapsula la configuración de dependencias de esta capa.
// Evita que la API necesite conocer detalles de implementación (como que usamos Dapper o SQL).
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Usamos Scoped porque la conexión a SQL y la transacción deben vivir
        // exactamente lo que dura el Request HTTP.
        // Singleton causaría colisiones de hilos; Transient abriría demasiadas conexiones.
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        return services;
    }
}