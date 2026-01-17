<div align="center">

# Payment Processor API Challenge

**[ 🇬🇧 English ](#-english) | [ 🇪🇸 Español ](#-español)**

</div>

---

<a name="-english"></a>
## 🇬🇧 English

High-transactionality payment processing API built with **.NET 8**, implementing **Clean Architecture** and robust design principles to ensure financial integrity and scalability.

### 🚀 Key Features

This project demonstrates solutions to complex fintech industry challenges:

* **Transactional Idempotency:** Implementation of `Idempotency-Key` headers to prevent double processing during network retries.
* **Concurrency Management:** Utilization of pessimistic locking (`UPDLOCK`) and database-level transaction isolation (`ReadCommitted`) to ensure balance consistency under parallel requests.
* **Clean Architecture:** Strict separation of concerns (Core, Infrastructure, API) to enhance maintainability and testability.
* **Performance:** usage of **Dapper** as a micro-ORM for optimized critical query performance.
* **Dockerized:** Full environment (API + SQL Server 2022) orchestrated via Docker Compose.

### 🛠️ Tech Stack

* **.NET 8** (C#)
* **SQL Server 2022** (Linux Container)
* **Dapper** (High-performance ORM)
* **Docker & Docker Compose**
* **xUnit** (Unit Testing)
* **Swagger / OpenAPI** (Documentation)

### ⚙️ How to Run

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/Godiel/payment-processor-challenge.git](https://github.com/Godiel/payment-processor-challenge.git)
    cd payment-processor-challenge
    ```

2.  **Start Infrastructure:**
    Run the following command to start SQL Server and initialize the database automatically:
    ```bash
    docker-compose up -d
    ```

3.  **Run the API:**
    ```bash
    dotnet run --project src/PaymentProcessor.Api
    ```

4.  **Explore & Test:**
    Open your browser at the address shown in the console (usually `http://localhost:5xxx/swagger`) to test the endpoints.

---

<a name="-español"></a>
## 🇪🇸 Español

API de procesamiento de pagos de alta transaccionalidad desarrollada con **.NET 8**, implementando **Clean Architecture** y principios de diseño robustos para garantizar integridad financiera y escalabilidad.

### 🚀 Características Principales

Este proyecto demuestra soluciones a problemas complejos de la industria fintech:

* **Idempotencia Transaccional:** Implementación de `Idempotency-Key` en headers para prevenir cobros duplicados en redes inestables.
* **Manejo de Concurrencia:** Uso de bloqueos pesimistas (`UPDLOCK`) y niveles de aislamiento de transacción (`ReadCommitted`) para asegurar la consistencia del saldo ante peticiones paralelas.
* **Clean Architecture:** Separación estricta de responsabilidades en capas (Core, Infraestructura, API) para facilitar el mantenimiento y las pruebas.
* **Performance:** Uso de **Dapper** como micro-ORM para optimizar el rendimiento de las consultas críticas.
* **Dockerized:** Entorno completo (API + SQL Server 2022) orquestado con Docker Compose.

### 🛠️ Stack Tecnológico

* **.NET 8** (C#)
* **SQL Server 2022** (Linux Container)
* **Dapper** (Acceso a datos)
* **Docker & Docker Compose**
* **xUnit** (Testing)
* **Swagger / OpenAPI** (Documentación)

### ⚙️ Cómo ejecutar el proyecto

1.  **Clonar el repositorio:**
    ```bash
    git clone [https://github.com/Godiel/payment-processor-challenge.git](https://github.com/Godiel/payment-processor-challenge.git)
    cd payment-processor-challenge
    ```

2.  **Levantar la infraestructura:**
    Ejecuta el siguiente comando para iniciar SQL Server y configurar la base de datos automáticamente:
    ```bash
    docker-compose up -d
    ```

3.  **Ejecutar la API:**
    ```bash
    dotnet run --project src/PaymentProcessor.Api
    ```

4.  **Explorar la documentación:**
    Abre tu navegador en la dirección indicada en la consola (usualmente `http://localhost:5xxx/swagger`) para ver y probar los endpoints.

---

<div align="center">

Developed by **[Anthonyjgn - Godiel](https://github.com/Godiel)**
<br>

</div>
