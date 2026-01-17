using System.Threading.Tasks;

namespace PaymentProcessor.Core.Interfaces;

public interface IPaymentRepository
{
    // Debe garantizar atomicidad: Debito + Registro de Log o falla total.
    Task<bool> ProcessPaymentAsync(int accountId, decimal amount, string idempotencyKey);

    Task<bool> ExistsAsync(string idempotencyKey);
}