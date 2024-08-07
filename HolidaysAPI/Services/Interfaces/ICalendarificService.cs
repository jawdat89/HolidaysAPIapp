using IHolidaysAPI.Models;

namespace IHolidaysAPI.Services;

public interface ICalendarificService
{
    Task<CalendarificHolidaysAPIResponse> GetCalendarificHolidaysAsync(string countryCode, string year);
}
