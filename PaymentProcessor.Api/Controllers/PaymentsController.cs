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

    public PaymentsController(IPaymentRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> Process([FromBody] PaymentRequest request, [FromHeader(Name = "X-Idempotency-Key")] string idempotencyKey)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey))
            return BadRequest(new { error = "El header 'X-Idempotency-Key' es obligatorio." });

        if (await _repository.ExistsAsync(idempotencyKey))
        {
            return Conflict(new { error = "Esta transacción ya fue procesada previamente." });
        }

        var success = await _repository.ProcessPaymentAsync(request.AccountId, request.Amount, idempotencyKey);

        if (!success)
        {
            return UnprocessableEntity(new { error = "Saldo insuficiente o cuenta inválida." });
        }

        return Ok(new { status = "Approved", transactionId = idempotencyKey });
    }
}