using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using WeatherArchive.Models;

namespace WeatherArchive.Services
{
    public interface IExcelService
    {
        Task<List<WeatherData>> ReadWeatherDataFromExcel(Stream fileStream);
    }

    public class ExcelService : IExcelService
    {
        public async Task<List<WeatherData>> ReadWeatherDataFromExcel(Stream fileStream)
        {
            List<WeatherData> weatherDataList = new();

            using var workbook = new XSSFWorkbook(fileStream);
            for (var sheetIndex = 0; sheetIndex < workbook.NumberOfSheets; sheetIndex++)
            {
                var sheet = workbook.GetSheetAt(sheetIndex);
                for (var i = 4; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    if (row != null)
                    {
                        var weatherData = new WeatherData();

                        var dateCell = row.GetCell(0);
                        var timeCell = row.GetCell(1);

                        if (dateCell != null && timeCell != null)
                        {
                            var dateValue = GetCellValueAsDateTime(dateCell);
                            var timeValue = GetCellValueAsDateTime(timeCell);

                            if (dateValue.HasValue && timeValue.HasValue)
                            {
                                var dateTime = dateValue.Value.Date.Add(timeValue.Value.TimeOfDay);
                                weatherData.DateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                            }
                        }

                        weatherData.Temperature = GetCellValue<double?>(row.GetCell(2));
                        weatherData.Humidity = GetCellValue<double?>(row.GetCell(3));
                        weatherData.DewPoint = GetCellValue<double?>(row.GetCell(4));
                        weatherData.Pressure = GetCellValue<double?>(row.GetCell(5));
                        weatherData.WindDirection = GetCellValue<string>(row.GetCell(6));
                        weatherData.WindSpeed = GetCellValue<double?>(row.GetCell(7));
                        weatherData.Cloudiness = GetCellValue<int?>(row.GetCell(8));
                        weatherData.CloudinessLower = GetCellValue<int?>(row.GetCell(9));
                        weatherData.Visibility = GetCellValue<double?>(row.GetCell(10));
                        weatherData.WeatherPhenomenon = GetCellValue<string>(row.GetCell(11));

                        weatherDataList.Add(weatherData);
                    }
                }
            }

            return weatherDataList;
        }

        public static T? GetCellValue<T>(ICell? cell)
        {
            if (cell == null)
                return default;

            if (typeof(T) == typeof(DateTime?))
            {
                if (cell.CellType == CellType.Numeric)
                    return (T)(object)cell.DateCellValue;
            }
            else if (typeof(T) == typeof(double?))
            {
                if (cell.CellType == CellType.Numeric)
                    return (T)(object)(double)cell.NumericCellValue;
            }
            else if (typeof(T) == typeof(int?))
            {
                if (cell.CellType == CellType.Numeric)
                    return (T)(object)(int)cell.NumericCellValue;
            }
            else if (typeof(T) == typeof(string))
            {
                if (cell.CellType == CellType.String)
                    return (T)(object)cell.StringCellValue;
            }

            return default;
        }

        private static DateTime? GetCellValueAsDateTime(ICell? cell)
        {
            if (cell == null)
                return null;

            switch (cell.CellType)
            {
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                        return cell.DateCellValue;
                    break;
                case CellType.String:
                    if (DateTime.TryParse(cell.StringCellValue, out var dateValue))
                        return dateValue;
                    break;
            }

            return null;
        }
    }
}

