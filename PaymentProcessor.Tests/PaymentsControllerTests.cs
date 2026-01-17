using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentProcessor.Api.Controllers;
using PaymentProcessor.Api.DTOs;
using PaymentProcessor.Core.Interfaces;
using Xunit;

namespace PaymentProcessor.Tests;

public class PaymentsControllerTests
{
    // Mock del repositorio: Simula la BD sin tocar SQL real.
    private readonly Mock<IPaymentRepository> _mockRepo;
    // El "SUT" (System Under Test)
    private readonly PaymentsController _controller;

    public PaymentsControllerTests()
    {
        _mockRepo = new Mock<IPaymentRepository>();
        _controller = new PaymentsController(_mockRepo.Object);
    }

    [Fact]
    public async Task Process_ShouldReturnBadRequest_WhenIdempotencyKeyIsMissing()
    {
        // Arrange
        var request = new PaymentRequest { AccountId = 1, Amount = 100 };

        // Act
        var result = await _controller.Process(request, ""); // Clave vacía

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Process_ShouldReturnConflict_WhenPaymentAlreadyExists()
    {
        // Arrange
        var key = "duplicate-key";
        // Configuramos el Mock para que diga "Sí, esta clave ya existe"
        _mockRepo.Setup(r => r.ExistsAsync(key)).ReturnsAsync(true);

        var request = new PaymentRequest { AccountId = 1, Amount = 100 };

        // Act
        var result = await _controller.Process(request, key);

        // Assert
        Assert.IsType<ConflictObjectResult>(result);

        // Verificamos que NUNCA se haya llamado al método de procesar pago
        _mockRepo.Verify(r => r.ProcessPaymentAsync(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Process_ShouldReturnUnprocessableEntity_WhenRepositoryReturnsFalse()
    {
        // Arrange (Simulamos saldo insuficiente)
        var key = "new-key";
        _mockRepo.Setup(r => r.ExistsAsync(key)).ReturnsAsync(false);
        _mockRepo.Setup(r => r.ProcessPaymentAsync(1, 5000, key)).ReturnsAsync(false);

        var request = new PaymentRequest { AccountId = 1, Amount = 5000 };

        // Act
        var result = await _controller.Process(request, key);

        // Assert
        var objectResult = Assert.IsType<UnprocessableEntityObjectResult>(result);
    }

    [Fact]
    public async Task Process_ShouldReturnOk_WhenPaymentIsSuccessful()
    {
        // Arrange (Happy Path)
        var key = "success-key";
        _mockRepo.Setup(r => r.ExistsAsync(key)).ReturnsAsync(false);
        _mockRepo.Setup(r => r.ProcessPaymentAsync(1, 100, key)).ReturnsAsync(true);

        var request = new PaymentRequest { AccountId = 1, Amount = 100 };

        // Act
        var result = await _controller.Process(request, key);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        // Verificamos que SÍ se llamó al repositorio una vez
        _mockRepo.Verify(r => r.ProcessPaymentAsync(1, 100, key), Times.Once);
    }
}