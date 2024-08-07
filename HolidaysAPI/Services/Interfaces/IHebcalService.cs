using IHolidaysAPI.Models;

namespace IHolidaysAPI.Services;

public interface IHebcalService
{
    Task<HebrewCalendarResponse> GetHebcalData(string year);
}
