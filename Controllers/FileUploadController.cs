using Microsoft.AspNetCore.Mvc;
using WeatherArchive.Services;

namespace WeatherArchive.Controllers
{
    public class UploadController : Controller
    {
        private readonly IExcelService _excelService;
        private readonly IWeatherDataService _weatherDataService;

        public UploadController(IExcelService excelService, IWeatherDataService weatherDataService)
        {
            _excelService = excelService;
            _weatherDataService = weatherDataService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            var failedFiles = new List<string>();

            foreach (var file in files)
            {
                if (Path.GetExtension(file.FileName).ToLower() != ".xlsx")
                {
                    failedFiles.Add(file.FileName);
                    continue;
                }

                try
                {
                    await using var fileStream = file.OpenReadStream();
                    var weatherDataList = await _excelService.ReadWeatherDataFromExcel(fileStream);
                    var result = await _weatherDataService.AddWeatherData(weatherDataList);
                    if (!result)
                        failedFiles.Add(file.FileName);
                }
                catch
                {
                    failedFiles.Add(file.FileName);
                }
            }

            if (failedFiles.Any())
                return View("Index", failedFiles);

            return RedirectToAction("Index", "WeatherData");
        }
    }
}
