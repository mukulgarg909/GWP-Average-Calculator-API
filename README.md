# GWP (Gross Written Premium) API Project Structure
GwpApi/
├── Controllers/       # API endpoints
├── Models/            # DTOs and entities
├── Repositories/      # Data access
├── Services/          # Business logic
├── Program.cs         # Startup configuration
└── appsettings.json   # Configuration
└── gwpByCountry.csv   # Sample Data

GwpApi.Tests/
├── UnitTests/         # Test cases

### Prerequisites
.NET SDK
Visual Studio IDE

### Installation
1. Clone the repository:
   git clone https://github.com/mukulgarg909/GWP-Average-Calculator-API.git
   cd gwp-api
2. dotnet restore
3. dotnet run --project GwpApi/GwpApi.csproj
   The API will be available at http://localhost:9091 (HTTP) or https://localhost:9092 (HTTPS)
4. dotnet test

## API Documentation (/swagger)
1. Sample Request:
   POST /server/api/gwp/avg
  Content-Type: application/json
  
  {
      "country": "ae",
      "lob": ["transport", "property"]
  }

2. Sample Output:
   {
    "lob": avg_value,
    "property": avg_value
   }
