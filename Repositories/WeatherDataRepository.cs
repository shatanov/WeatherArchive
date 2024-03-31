using Microsoft.EntityFrameworkCore;
using WeatherArchive.DbContext;
using WeatherArchive.Models;

namespace WeatherArchive.Repositories
{
    public interface IWeatherDataRepository
    {
        Task<bool> AddWeatherData(List<WeatherData> weatherDataList);
        Task<IEnumerable<WeatherData>> GetWeatherData(int? year, int? month, int page, int pageSize);
        Task<long> GetWeatherDataCount(int? year, int? month);
    }
    public class WeatherDataRepository : IWeatherDataRepository
    {
        private readonly ApplicationDbContext _context;

        public WeatherDataRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddWeatherData(List<WeatherData> weatherDataList)
        {
            try
            {
                await _context.AddRangeAsync(weatherDataList);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding weather data: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<WeatherData>> GetWeatherData(int? year, int? month, int page, int pageSize)
        {
            IQueryable<WeatherData> query = _context.WeatherData;

            if (year.HasValue)
            {
                query = query.Where(w => w.DateTime.HasValue && w.DateTime.Value.Year == year);
            }

            if (month.HasValue)
            {
                query = query.Where(w => w.DateTime.HasValue && w.DateTime.Value.Month == month);
            }

            var weatherData = await query
                .OrderByDescending(w => w.DateTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return weatherData;
        }

        public async Task<long> GetWeatherDataCount(int? year, int? month)
        {
            IQueryable<WeatherData> query = _context.WeatherData;

            if (year.HasValue)
            {
                query = query.Where(w => w.DateTime.HasValue && w.DateTime.Value.Year == year);
            }

            if (month.HasValue)
            {
                query = query.Where(w => w.DateTime.HasValue && w.DateTime.Value.Month == month);
            }
            return await query.CountAsync();
        }
    }
}
