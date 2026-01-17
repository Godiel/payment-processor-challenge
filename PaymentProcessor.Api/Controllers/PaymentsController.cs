using Microsoft.AspNetCore.Mvc;
using PaymentProcessor.Api.DTOs;
using PaymentProcessor.Core.Interfaces;
using PaymentProcessor.Infrastructure.Repositories;

namespace PaymentProcessor.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentRepository _repository;

    // Inyección de dependencias sobre la interfaz (DIP), no la implementación.
    // Esto desacopla la API de la base de datos específica.
    public PaymentsController(IPaymentRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Procesa una transacción de débito garantizando idempotencia.
    /// </summary>
    /// <param name="request">Datos del pago (Cuenta y Monto).</param>
    /// <param name="idempotencyKey">Header crítico para evitar duplicidad en reintentos.</param>
    [HttpPost]
    public async Task<IActionResult> Process([FromBody] PaymentRequest request, [FromHeader(Name = "X-Idempotency-Key")] string idempotencyKey)
    {
        // Fail-fast: Validación de protocolo antes de tocar infraestructura.
        if (string.IsNullOrWhiteSpace(idempotencyKey))
            return BadRequest(new { error = "El header 'X-Idempotency-Key' es obligatorio." });

        // Patrón Check-Then-Act para Idempotencia.
        // Si la llave existe, devolvemos 409 Conflict para indicar al cliente que no reintente ciegamente.
        if (await _repository.ExistsAsync(idempotencyKey))
        {
            return Conflict(new { error = "Esta transacción ya fue procesada previamente." });
        }

        // Delegamos la complejidad transaccional (Bloqueos/SQL) a la capa de Infraestructura.
        // El controlador se mantiene "Thin" (fino).
        var success = await _repository.ProcessPaymentAsync(request.AccountId, request.Amount, idempotencyKey);

        if (!success)
        {
            // Retornamos 422 Unprocessable Entity en lugar de 400 Bad Request.
            // Significa: "Entendí tu petición (sintaxis OK), pero no puedo procesarla por reglas de negocio (saldo insuficiente)".
            return UnprocessableEntity(new { error = "Saldo insuficiente o cuenta inválida." });
        }

        return Ok(new { status = "Approved", transactionId = idempotencyKey });
    }
}