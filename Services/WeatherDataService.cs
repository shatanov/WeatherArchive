using WeatherArchive.DbContext;
using WeatherArchive.Models;
using WeatherArchive.Repositories;

namespace WeatherArchive.Services;

public interface IWeatherDataService
{
    Task<bool> AddWeatherData(List<WeatherData> weatherDataList);
    Task<IEnumerable<WeatherData>> GetWeatherData(int? year, int? month, int page, int pageSize);
    Task<long> GetWeatherDataCount(int? year, int? month);
}

public class WeatherDataService : IWeatherDataService
{
    private readonly IWeatherDataRepository _weatherDataRepository;

    public WeatherDataService(IWeatherDataRepository weatherDataRepository)
    {
        _weatherDataRepository = weatherDataRepository;
    }

    public async Task<bool> AddWeatherData(List<WeatherData> weatherDataList)
    {
        return await _weatherDataRepository.AddWeatherData(weatherDataList);
    }

    public async Task<IEnumerable<WeatherData>> GetWeatherData(int? year, int? month, int page, int pageSize)
    {
        return await _weatherDataRepository.GetWeatherData(year, month, page, pageSize);
    }

    public async Task<long> GetWeatherDataCount(int? year, int? month)
    {
        return await _weatherDataRepository.GetWeatherDataCount(year, month);
    }
}