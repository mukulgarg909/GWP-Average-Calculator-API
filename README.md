# GWP (Gross Written Premium) API Project

### Prerequisites
.NET SDK
Visual Studio IDE

### Installation
1. Clone the repository:
   git clone https://github.com/mukulgarg909/GWP-Average-Calculator-API.git
   cd gwp-api
2. dotnet restore
3. dotnet run --project GwpApi/GwpApi.csproj
   (The API will be available at http://localhost:9091 (HTTP) or https://localhost:9092 (HTTPS))
4. dotnet test
   
### Structure
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

## API Documentation (/swagger)
1. Sample Request:
   POST /server/api/gwp/avg
  Content-Type: application/json
  
  {
      "country": "xx",
      "lob": ["lob1", "lob2"]
  }

2. Sample Output:
   {
    "lob1": avg_lob1_value,
    "lob2": avg_lob2_value
   }
