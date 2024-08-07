using System.Text.Json.Serialization;

namespace HolidaysAPI.Models;

public class CalendarificHolidaysResponse
{
    public CalendarificHoliday[] Holidays { get; set; } = [];
}

public class CalendarificMeta
{
    public int Code { get; set; }
}

public class CalendarificHolidaysAPIResponse
{
    public CalendarificMeta Meta { get; set; }
    public CalendarificHolidaysResponse Response { get; set; }
} 

public class CalendarificHoliday
{
    public string Name { get; set; }
    public string Description { get; set; }
    public CalendarificCountry Country { get; set; }
    public CalendarificDateModel Date { get; set; }
    public string[] Type { get; set; }
    [JsonPropertyName("primary_type")]
    public string PrimaryType { get; set; }
    [JsonPropertyName("canonical_url")]
    public string CanonicalUrl { get; set; }
    [JsonPropertyName("urlid")]
    public string URLId { get; set; }
    public string Locations { get; set; }
    public string States { get; set; }
}

public class CalendarificDateModel
{
    [JsonPropertyName("iso")]
    public string ISO { get; set; }
    [JsonPropertyName("datetime")]
    public CalendarificDate DateTime { get; set; }
}

public class CalendarificDate
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
}

public class CalendarificCountry
{
    public string Id { get; set; }
    public string Name { get; set; }
}