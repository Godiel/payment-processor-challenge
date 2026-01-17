using System.Threading.Tasks;

namespace PaymentProcessor.Core.Interfaces;

public interface IPaymentRepository
{
    Task<bool> ProcessPaymentAsync(int accountId, decimal amount, string idempotencyKey);

    Task<bool> ExistsAsync(string idempotencyKey);
}