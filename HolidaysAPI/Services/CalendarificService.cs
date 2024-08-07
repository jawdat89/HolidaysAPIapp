using HolidaysAPI.Models;
using System.Net.Http;
using System.Text.Json;

namespace HolidaysAPI.Services
{
    public class CalendarificService : ICalendarificService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<CalendarificService> _logger;
        private readonly HttpClient _httpClient;

        public CalendarificService(IConfiguration config, ILogger<CalendarificService> logger, HttpClient httpClient)
        {
            _config = config;
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<CalendarificHolidaysAPIResponse> GetCalendarificHolidaysAsync(string countryCode, string year)
        {
            try
            {
                // Retrieve the Calendarific base URL from the configuration
                var calendarificBaseUrl = _config.GetSection("External:Calendarific:URL").Value!;
                var calendarificAPIKey = _config.GetSection("External:Calendarific:APIKey").Value!;
                // Construct the request URI with all required query parameters
                var requestUri = $"{calendarificBaseUrl}/holidays?&api_key={calendarificAPIKey}&country={countryCode}&year={year}";

                // Make the HTTP GET request to the Calendarific API
                using HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    // Log and throw an exception if the response status code is not successful
                    var errorMessage = $"Calendarific API request failed with status code: {response.StatusCode}";
                    _logger.LogError(errorMessage);
                    throw new HttpRequestException(errorMessage);
                }

                // Read the string content of the response
                var content = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON content to the HebrewCalendarResponse model
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var calendarificData = JsonSerializer.Deserialize<CalendarificHolidaysAPIResponse>(content, options);

                // Return the deserialized data
                return calendarificData;
            }
            catch (Exception ex)
            {
                // Log and re-throw any exception that occurs during the process
                var message = $"[CalendarificService] - failed to process GetCalendarificData: {ex.Message}";
                _logger.LogError(message);
                throw new Exception(message);
            }
        }
    }
}
