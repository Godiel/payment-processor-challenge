using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PaymentProcessor.Core.Entities;
using PaymentProcessor.Core.Interfaces;

namespace PaymentProcessor.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly string _connectionString;

    public PaymentRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? throw new ArgumentNullException("Connection string missing");
    }

    public async Task<bool> ExistsAsync(string idempotencyKey)
    {
        using var connection = new SqlConnection(_connectionString);
        // SELECT 1 es suficiente para verificar existencia (más performante que count).
        const string sql = "SELECT 1 FROM Payments WHERE IdempotencyKey = @Key";
        var result = await connection.ExecuteScalarAsync<int?>(sql, new { Key = idempotencyKey });
        return result.HasValue;
    }

    public async Task<bool> ProcessPaymentAsync(int accountId, decimal amount, string idempotencyKey)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        // IsolationLevel.ReadCommitted + UPDLOCK garantiza consistencia sin bloquear toda la tabla.
        using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

        try
        {
            // UPDLOCK: Bloqueo pesimista para evitar condiciones de carrera en lectura-escritura.
            // Mantiene la fila bloqueada hasta el commit.
            const string sqlCheck = @"
                SELECT Balance 
                FROM Accounts WITH (UPDLOCK) 
                WHERE Id = @AccountId";

            var balance = await connection.QuerySingleOrDefaultAsync<decimal?>(sqlCheck, new { AccountId = accountId }, transaction);

            if (balance == null || balance < amount)
            {
                return false; // Validación de negocio fallida.
            }

            const string sqlUpdate = @"
                UPDATE Accounts 
                SET Balance = Balance - @Amount 
                WHERE Id = @AccountId";

            await connection.ExecuteAsync(sqlUpdate, new { Amount = amount, AccountId = accountId }, transaction);

            const string sqlInsert = @"
                INSERT INTO Payments (AccountId, Amount, Status, IdempotencyKey)
                VALUES (@AccountId, @Amount, 'Completed', @Key)";

            await connection.ExecuteAsync(sqlInsert, new { AccountId = accountId, Amount = amount, Key = idempotencyKey }, transaction);

            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}