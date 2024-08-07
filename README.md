# HolidaysAPIapp

## Overview

HolidaysAPIapp is a .NET API project designed to provide holiday information using multiple calendar services. The API integrates with Calendarific and Hebcal to fetch holiday data for various regions and calendar systems.

## Features

- Retrieve holiday information from Calendarific.
- Retrieve Jewish holiday information from Hebcal.
- Support for multiple calendar types and regions.

## Prerequisites

- .NET 6.0 SDK or later
- API key for Calendarific
- API key for Hebcal (if required)

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/jawdat89/HolidaysAPIapp.git
cd HolidaysAPIapp
```
### Configure the Application
- Open the appsettings.json file and add your API keys for Calendarific and Hebcal, It's better to use User Secrets for security matters:
{
  "External": { // Use User Secrets to store these values instead of hardcoding them here
    "Hebcal": "https://www.hebcal.com/hebcal", // URL for Hebcal API
    "Calendarific": {
      "URL": "https://calendarific.com/api/v2", // URL for Calendarific API
      "APIKey": "CALENDARIFIC_API_KEY" // API key for Calendarific API
    }
  }
}

## API Endpoints
### Calendarific Service
- Get Holidays
```bash
GET /api/calendarific/holidays?country=US&year=2024
```
### Hebcal Service
- Get Jewish Holidays
```bash
GET /api/hebcal/jewish-holidays?year=2024
```
### License
This project is licensed under the MIT License.


