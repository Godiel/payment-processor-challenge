using System.ComponentModel.DataAnnotations;

namespace PaymentProcessor.Api.DTOs;

public record PaymentRequest
{
    [Required]
    public int AccountId { get; init; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
    public decimal Amount { get; init; }
}