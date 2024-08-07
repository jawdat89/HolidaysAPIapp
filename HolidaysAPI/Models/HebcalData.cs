using System;
using System.Text.Json.Serialization;

namespace IHolidaysAPI.Models;

/// <summary>
/// Represents the response from the Hebcal Jewish Calendar API.
/// </summary>
public class HebrewCalendarResponse
{
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public Location Location { get; set; }
    public Range Range { get; set; }
    public List<HolidayEvent> Items { get; set; }
}

/// <summary>
/// Represents the geographical location information in the response.
/// </summary>
public class Location
{
    public string? Title { get; set; }
    public string? City { get; set; }
    [JsonPropertyName("tzid")]
    public string? TZId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    [JsonPropertyName("cc")]
    public string? CC { get; set; }
    public string? Country { get; set; }
    public int Elevation { get; set; }
    public string? Admin1 { get; set; }
    [JsonPropertyName("asciiname ")]
    public string? ASCIIName { get; set; }
    public string? Geo { get; set; }
    [JsonPropertyName("geonameid")]
    public int GeoNameId { get; set; }


}

/// <summary>
/// Represents an individual holiday or event from the Hebcal API.
/// </summary>
public class HolidayEvent
{
    public string Title { get; set; }
    public DateTime Date { get; set; } // could be also DateOnly
    public string Category { get; set; }
    [JsonPropertyName("title_orig")]
    public string? TitleOrig { get; set; }
    public string? Hebrew { get; set; }
    public string? Memo { get; set; }
    public Dictionary<string, string>? Leyning { get; set; }
    public string? Link { get; set; }
    public string? Subcat { get; set; }
    public bool? Yomtov { get; set; }
    [JsonPropertyName("hdate")]
    public string? HDate { get; set; }
    // Additional fields as necessary based on the API's response
}

/// <summary>
/// Represents the range of the data requested
/// </summary>
public class Range
{
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
}

public class CategorizedHebcalHolidaysPayload
{
    public string Year { get; set; }
    public string[] Categories { get; set; }
}