using HolidaysAPI.Models;

namespace HolidaysAPI.Services;

public interface ICalendarificService
{
    Task<CalendarificHolidaysAPIResponse> GetCalendarificHolidaysAsync(string countryCode, string year);
}
