namespace PaymentProcessor.Core.Entities;

public class Account
{
    public int Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string Currency { get; set; } = "USD";

    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}