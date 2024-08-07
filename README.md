HolidaysAPIapp
Overview
HolidaysAPIapp is a .NET API project designed to provide holiday information using multiple calendar services. The API integrates with Calendarific and Hebcal to fetch holiday data for various regions and calendar systems.

Features
Retrieve holiday information from Calendarific.
Retrieve Jewish holiday information from Hebcal.
Support for multiple calendar types and regions.
Prerequisites
.NET 6.0 SDK or later
API key for Calendarific
API key for Hebcal (if required)
Getting Started
Clone the Repository
bash
Copy code
git clone https://github.com/jawdat89/HolidaysAPIapp.git
cd HolidaysAPIapp
Configure the Application
Open the appsettings.json file and add your API keys for Calendarific and Hebcal:

json
Copy code
{
  "Calendarific": {
    "ApiKey": "YOUR_CALENDARIFIC_API_KEY",
    "BaseUrl": "https://calendarific.com/api/v2"
  },
  "Hebcal": {
    "ApiKey": "YOUR_HEBCAL_API_KEY", // If required
    "BaseUrl": "https://www.hebcal.com"
  }
}
Save the appsettings.json file.

Build and Run the Application
Build the project:

bash
Copy code
dotnet build
Run the project:

bash
Copy code
dotnet run
API Endpoints
Calendarific Service
Get Holidays

http
Copy code
GET /api/calendarific/holidays?country=US&year=2024
Parameters:

country (string): The country code (e.g., US).
year (int): The year for which to retrieve holidays (e.g., 2024).
Hebcal Service
Get Jewish Holidays

http
Copy code
GET /api/hebcal/jewish-holidays?year=2024
Parameters:

year (int): The year for which to retrieve Jewish holidays (e.g., 2024).
Project Structure
Controllers: Contains the API controllers for Calendarific and Hebcal services.
Services: Contains the service classes that interact with Calendarific and Hebcal APIs.
Models: Contains the data models used in the project.
Configuration: Contains configuration classes for external services.
Example Requests
Calendarific Service
Request:

http
Copy code
GET /api/calendarific/holidays?country=US&year=2024
Response:

json
Copy code
{
  "holidays": [
    {
      "name": "New Year's Day",
      "date": "2024-01-01",
      "description": "The first day of the year",
      "country": "US"
    },
    // More holidays...
  ]
}
Hebcal Service
Request:

http
Copy code
GET /api/hebcal/jewish-holidays?year=2024
Response:

json
Copy code
{
  "holidays": [
    {
      "name": "Rosh Hashanah",
      "date": "2024-10-02",
      "description": "The Jewish New Year",
      "category": "Jewish"
    },
    // More holidays...
  ]
}
Contributing
Fork the repository.
Create a new branch.
Make your changes.
Submit a pull request.
License
This project is licensed under the MIT License.
