# Dental Mock UI

## Description
This project is an ASP.NET Core API which simulates UI for sending requests to eReferral API through the APIM using SwaggerUI.

## Prerequisites
Make sure you have the following installed and set up:
- [.NET SDK](https://dotnet.microsoft.com/download) version 8.0
- `az login --tenant <YOUR_TENNANT>`

## Required configuration for local development
To configure the project, open user secrets file and configure ApimEndpoint and ApimSubscriptionKey.
```
{
  "EReferralsApi": {
    "ApimEndpoint": "<YOUR_APIM_ENDPOINT>"
  }
}
```

## Project Structure
The core project structure is organized as follows:
```
WCCG.DentalMock.UI/
│
├── Properties
│   └── launchSettings.json
|
├── Configuration
│   └── Configuration files and their validation
│
├── Controllers
|   └── Controllers for API
|
├── Errors
│   └── FHIR HTTP error models
|
├── Middleware
│   └── Response finalisation and error handling process
|
├── Services
│   └── Service classes
|
├── Swagger
│   └── Helper classes for Swagger
|
├── appsettings.json
|   └── appsettings.Development.json
|
└── Program.cs
```

## Running the Project
To run the project locally, follow these steps:
1. Clone the repository.
2. Don't forget `az login --tenant <YOUR_TENNANT>`
3. Setup local configuration according to `Required configuration for local development` section
4. Rebuild and run the project.
5. Open your web browser and navigate to `https://localhost:xxxxx/swagger/index.html` to access the SwaggerUI with API endpoints.

## API Endpoints
Example payloads for POST endpoints can be found in the `Examples` folder. 

### POST /api/Referrals
- Description: Creates a referral and returns enriched response 
- Request body should be a valid FHIR Bundle JSON object. [Example Payload](./src/WCCG.DentalMock.UI/Examples/createReferral-example-payload.json)
- Request should contain required header values described in [RequestHeaderKeys.cs](./src/WCCG.DentalMock.UI/Constants/RequestHeaderKeys.cs)
- Response is also a FHIR Bundle but enriched with new values generated while the creation process

### GET /api/Referrals/&#123;referralId&#125;
- Description: Gets a referral by **referralId**.
- Route parameter **referralId** should be a valid GUID.
- Response is a FHIR Bundle generated based on database data.

