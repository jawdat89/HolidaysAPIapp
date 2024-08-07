using HolidaysAPI.Models;

namespace HolidaysAPI.Services;

public interface IHebcalService
{
    Task<HebrewCalendarResponse> GetHebcalData(string year);
}
