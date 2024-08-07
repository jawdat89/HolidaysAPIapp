using HolidaysAPI.Models;
using HolidaysAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HolidaysAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidaysController : ControllerBase
    {
        private readonly IHebcalService _hebcalService;
        private readonly ICalendarificService _calendarificService;

        public HolidaysController(IHebcalService hebcalService, ICalendarificService calendarificService)
        {
            _hebcalService = hebcalService;
            _calendarificService = calendarificService;
        }

        [HttpGet("GetHebcalHolidays({year})")]
        public async Task<ActionResult<string>> GetHebcalHolidays(string year)
        {
            try
            {
                var allHebcalData = await _hebcalService.GetHebcalData(year);

                if (allHebcalData.Items == null)
                {
                    return NoContent();
                }

                return Ok(allHebcalData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("GetHebcalHolidaysByCategories")]
        public async Task<ActionResult<string>> GetHebcalHolidaysByCategories([FromBody]CategorizedHebcalHolidaysPayload payload)
        {
            try
            {
                var allHebcalData = await _hebcalService.GetHebcalData(payload.Year);

                if (allHebcalData.Items == null)
                {
                    return NoContent();
                }

                var HebcalDataItemsFilteredByCategories = allHebcalData.Items
                                                            .Where(i => payload.Categories.Contains(i.Category))
                                                            .ToList();

                HebrewCalendarResponse hebrewCalendarResponse = new()
                {
                    Title = allHebcalData.Title,
                    Date = allHebcalData.Date,
                    Location = allHebcalData.Location,
                    Range = allHebcalData.Range,
                    Items = HebcalDataItemsFilteredByCategories
                };

                return Ok(hebrewCalendarResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("GetCalendarificHolidaysByYear({year})")]
        public async Task<ActionResult<CalendarificHolidaysAPIResponse>> GetCalendarificHolidaysByYear(string year)
        {
            try
            {
                var cleanedYearValue = new string(year.Where(char.IsDigit).ToArray());

                // Fetch Jewish holidays from Israel
                var jewishHolidaysData = await _calendarificService.GetCalendarificHolidaysAsync("il", cleanedYearValue);

                // Fetch Muslim holidays from the UAE
                var muslimHolidaysData = await _calendarificService.GetCalendarificHolidaysAsync("sa", cleanedYearValue);
                var muslimHolidaysFiltered = muslimHolidaysData.Response.Holidays
                    .Where(h => h.Type.Contains("Observance"))
                    .ToArray();

                // Fetch Christian holidays
                var christianHolidaysData = await _calendarificService.GetCalendarificHolidaysAsync("va", cleanedYearValue);
                var christianHolidaysFiltered = christianHolidaysData.Response.Holidays
                    .Where(h => h.Type.Contains("Observance"))
                    .ToArray();

                // Combine and remove duplicates if necessary
                var combinedHolidays = jewishHolidaysData.Response.Holidays
                    //.Concat(muslimHolidaysFiltered)
                    //.Concat(christianHolidaysFiltered)
                    .Concat(muslimHolidaysData.Response.Holidays)
                    .Concat(christianHolidaysData.Response.Holidays)
                    .Distinct() // You might need a more complex logic for Distinct if objects can't be compared directly
                    .ToArray();

                var response = new CalendarificHolidaysAPIResponse
                {
                    Response = new CalendarificHolidaysResponse { Holidays = combinedHolidays }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
