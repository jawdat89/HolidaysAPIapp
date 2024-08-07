using IHolidays.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IHolidaysAPI.Services;

/// <summary>
/// Service for interacting with the Hebcal API to retrieve Jewish calendar data.
/// </summary>
public class HebcalService : IHebcalService
{
    private readonly IConfiguration _config;
    private readonly ILogger<HebcalService> _logger;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="HebcalService"/> class.
    /// </summary>
    /// <param name="config">The configuration object.</param>
    /// <param name="logger">The logger object.</param>
    /// <param name="httpClient">The HTTP client object.</param>
    public HebcalService(IConfiguration config, ILogger<HebcalService> logger, HttpClient httpClient)
    {
        _config = config;
        _logger = logger;
        _httpClient = httpClient;
    }

    /// <summary>
    /// Gets the Hebcal data for the specified year.
    /// </summary>
    /// <param name="year">The year for which to retrieve data.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="HebrewCalendarResponse"/> object.</returns>
    /// <exception cref="HttpRequestException">Thrown when the Hebcal API request fails.</exception>
    /// <exception cref="Exception">Thrown when there is an error processing the data.</exception>
    public async Task<HebrewCalendarResponse> GetHebcalData(string year)
    {
        try
        {
            // Retrieve the Hebcal base URL from the configuration
            var hebcalBaseUrl = _config.GetSection("External:Hebcal").Value!;
            // Construct the request URI with all required query parameters
            var requestUri = $"{hebcalBaseUrl}?v=1&cfg=json&maj=on&min=on&mod=on&nx=on&year={year}&month=x&ss=on&mf=on&c=on&geo=geoname&geonameid=281184&M=on&s=on";

            // Make the HTTP GET request to the Hebcal API
            using HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
            {
                // Log and throw an exception if the response status code is not successful
                var errorMessage = $"Hebcal API request failed with status code: {response.StatusCode}";
                _logger.LogError(errorMessage);
                throw new HttpRequestException(errorMessage);
            }

            // Read the string content of the response
            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Hebcal API response: {content}", content);

            // Deserialize the JSON content to the HebrewCalendarResponse model
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new DictionaryConverter() }
            };
            var hebcalData = JsonSerializer.Deserialize<HebrewCalendarResponse>(content, options);

            // Return the deserialized data
            return hebcalData;
        }
        catch (Exception ex)
        {
            // Log and re-throw any exception that occurs during the process
            var message = $"[HebcalService] - failed to process GetHebcalData: {ex.Message}";
            _logger.LogError(message);
            throw new Exception(message);
        }
    }

    /// <summary>
    /// Custom JSON converter for dictionaries where the values may be nested objects.
    /// </summary>
    private class DictionaryConverter : JsonConverter<Dictionary<string, string>>
    {
        /// <summary>
        /// Reads the JSON representation of the dictionary.
        /// </summary>
        /// <param name="reader">The reader object.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">The serializer options.</param>
        /// <returns>The dictionary with string keys and values.</returns>
        /// <exception cref="JsonException">Thrown when the JSON structure is invalid.</exception>
        public override Dictionary<string, string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var dictionary = new Dictionary<string, string>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return dictionary;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                var key = reader.GetString();
                reader.Read();

                string value;
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    using (JsonDocument document = JsonDocument.ParseValue(ref reader))
                    {
                        value = document.RootElement.GetRawText();
                    }
                }
                else
                {
                    value = reader.GetString();
                }

                dictionary.Add(key, value);
            }

            throw new JsonException();
        }

        /// <summary>
        /// Writes the dictionary to JSON.
        /// </summary>
        /// <param name="writer">The writer object.</param>
        /// <param name="value">The dictionary to write.</param>
        /// <param name="options">The serializer options.</param>
        public override void Write(Utf8JsonWriter writer, Dictionary<string, string> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (var kvp in value)
            {
                writer.WritePropertyName(kvp.Key);
                writer.WriteStringValue(kvp.Value);
            }
            writer.WriteEndObject();
        }
    }
}
