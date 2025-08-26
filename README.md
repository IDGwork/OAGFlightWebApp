# OAGFlightWebApp

A .NET 8 Web API that reads flight and pricing data from a public JSON API, computes valid roundtrip combinations, and exposes multiple queryable endpoints.  
It uses clean layering with **Repositories**, **Services**, and **DTOs**, and follows the **Repository Pattern** to decouple business logic from transport/data access.

---

## 📦 What it does

- Fetches raw journey and price data from:  
  `http://homeworktask.infare.lt/search.php?...`
- Combines inbound/outbound flights into roundtrips
- Computes:
  - All valid roundtrips
  - Cheapest roundtrip per route
  - Inbound duplicates with different prices
  - Total price grouped by route key
- Exposes REST API endpoints with Swagger documentation

---

## 🗂 Project structure

OAGFlightWebApp/
├─ Controllers/
│  └─ FlightController.cs
├─ DTOs/
│  ├─ FlightDto.cs
│  ├─ JourneyDto.cs
│  ├─ PriceResultDto.cs
│  └─ RoundtripDto.cs
├─ Models/
│  ├─ Availability.cs
│  ├─ Flight.cs
│  └─ Journey.cs
├─ Repositories/
│  ├─ IFlightRepository.cs
│  └─ FlightRepository.cs
├─ Services/
│  ├─ IFlightAnalysisService.cs
│  └─ FlightAnalysisService.cs
├─ appsettings.json
├─ Program.cs
└─ OAGFlightWebApp.csproj

> Note: Models represent parsed data; DTOs shape the response objects for API output. All business logic lives in `FlightAnalysisService`.

---

## ✅ Requirements

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download)
- No external database or storage needed
- Internet access (to call the external flight API)

---

## ▶️ Run

From the project root:

```bash
dotnet run

Then navigate to:

http://localhost:5299/swagger

You’ll see a Swagger UI listing all endpoints.

⸻

🔁 Endpoints

Route	Description
GET /api/flights/roundtrips	All valid roundtrip combinations
GET /api/flights/cheapest	Cheapest roundtrip per unique route
GET /api/flights/duplicates	Same inbound but different prices
GET /api/flights/grouped-by-price	Total prices grouped by roundtrip route

Use curl or Postman:

curl http://localhost:5299/api/flights/roundtrips
curl http://localhost:5299/api/flights/cheapest
curl http://localhost:5299/api/flights/duplicates
curl http://localhost:5299/api/flights/grouped-by-price


⸻

🧪 Tests

⚠️ No unit tests yet, but structure supports adding them.

To add:
	•	Create OAGFlightWebApp.Tests/
	•	Use xUnit + Moq for mocking IFlightRepository
	•	Add integration tests using Microsoft.AspNetCore.Mvc.Testing

⸻

💡 Troubleshooting
	•	No data? Ensure you’re online — the app pulls directly from the JSON endpoint.
	•	Swagger not showing? Rebuild and relaunch using dotnet run.
	•	Want to change airports or dates? Modify the URL in FlightRepository.cs.

⸻

📄 License

For assessment/demo purposes.
