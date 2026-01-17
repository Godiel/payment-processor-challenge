namespace PaymentProcessor.Core.Entities;

public class Account
{
    public int Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string Currency { get; set; } = "USD";

    // Manejo de concurrencia optimista (si decidimos quitar el bloqueo pesimista a futuro).
    // Mapea a timestamp/rowversion en SQL Server.
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}