# UseCase-01: Call External API

A sample **.NET 8 Web API** demonstrating resilient API calls using:
- `HttpClientFactory` (Typed Clients)
- `Polly` retry policies (with exponential backoff)
- Middleware for request logging
- Example integration with the **JSONPlaceholder** fake API

This project is part of my personal "Use Cases" series, where I practice common .NET interview scenarios.

---

## 📂 Project Structure

├── Abstractions/             # Interface
├── Clients/                  # HTTP client
├── Controllers/              # Posts/Users Controller
├── DTOs/                     # CreatePostDto
├── Errors/                   # ApiErrorResponse
├── Extensions/               # ApplicationServiceExtensions - service registrations
├── Middlewares/              # RequestLoggingMiddleware
├── Models/                   # Models for post, user
├── Programs.cs               # Start, service/Middleware registrations
└── usecase-01-call-external-api.http  # API testing file

--- 

## 🔧 Key Implementation Details

### HttpClientFactory (Typed Client)
- Strongly-typed client for external API communication  
- Separation of concerns (easier to test/mock)  
- Type-safe API calls  

### Retry Policy with Polly
- Exponential backoff retry strategy  
- Handles transient failures (e.g., `HttpRequestException`, `404 Not Found`)  
- Configurable retry attempts  

---

## 🚀 Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/suraj-varade/usecase-01-call-external-api.git

2. Navigate to project:
   cd usecase-01-call-external-api

3. Restore & Run the project:
   dotnet restore
   dotnet run
   Or
   open the solution file (.sln) in Visual Studio, Visual Studio Code, or Rider, and run the project from there. 

5. Test endpoints using Swagger UI or the provided .http file.

---

## Notes
- The POST API integration with JSONPlaceholder is only a mock – it accepts requests but does not persist data.

--- 

## Future Improvements
- Add circuit breaker policy with Polly for better fault tolerance
- Implement caching (e.g., in-memory, Redis) to reduce external API calls
- Introduce Clean Architecture layers (Core, Infrastructure, API)
- Add unit/integration tests for clients and controllers
- Integrate with a real API instead of JSONPlaceholder
- Add Docker support for containerized deployments
- Implement observability (structured logging, metrics, tracing)

---

## 📜 License
- This project is licensed under the MIT License. See the LICENSE file.

Note: Circuit breaker pattern not implemented in this version
