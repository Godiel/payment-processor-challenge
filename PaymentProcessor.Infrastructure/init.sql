IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'PaymentAuthDB')
BEGIN
    CREATE DATABASE PaymentAuthDB;
END
GO

USE PaymentAuthDB;
GO

-- Tabla de Cuentas (Simula la cuenta bancaria del usuario)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Accounts')
BEGIN
    CREATE TABLE Accounts (
        Id INT PRIMARY KEY IDENTITY(1,1),
        AccountNumber NVARCHAR(20) UNIQUE NOT NULL,
        Balance DECIMAL(18, 4) NOT NULL CHECK (Balance >= 0), 
        Currency CHAR(3) NOT NULL,
        RowVersion ROWVERSION NOT NULL 
    );

    -- Insertamos una cuenta de prueba con 1000 USD
    INSERT INTO Accounts (AccountNumber, Balance, Currency) 
    VALUES ('CUENTA-TEST-001', 1000.00, 'USD');
END
GO

-- Tabla de Pagos (Historial)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Payments')
BEGIN
    CREATE TABLE Payments (
        Id INT PRIMARY KEY IDENTITY(1,1),
        AccountId INT NOT NULL FOREIGN KEY REFERENCES Accounts(Id),
        Amount DECIMAL(18, 4) NOT NULL,
        Status NVARCHAR(20) NOT NULL,
        IdempotencyKey NVARCHAR(50) UNIQUE NOT NULL, 
        CreatedAt DATETIME2 DEFAULT GETUTCDATE()
    );
END
GO