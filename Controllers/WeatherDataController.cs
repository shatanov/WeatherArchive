using Microsoft.AspNetCore.Mvc;
using WeatherArchive.Services;

namespace WeatherArchive.Controllers
{
    public class WeatherDataController : Controller
    {
        private const int PageSize = 10;
        private readonly IWeatherDataService _weatherService;

        public WeatherDataController(IWeatherDataService weatherDataService)
        {
            _weatherService = weatherDataService;
        }

        public async Task<IActionResult> Index(int? year, int? month, int page = 1)
        {
            var weatherDataCount = await _weatherService.GetWeatherDataCount(year, month);
            var weatherData = await _weatherService.GetWeatherData(year, month, page, PageSize);
            var totalPages = (int)Math.Ceiling((double)weatherDataCount / PageSize);

            if (year.HasValue)
            {
                ViewBag.Year = year;
            }

            if (month.HasValue)
            {
                ViewBag.Month = month;
            }

            ViewBag.TotalPages = totalPages;
            ViewBag.Page = page;
            return View(weatherData);
        }
    }
}
