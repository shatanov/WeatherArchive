namespace WeatherArchive.Models;

public class WeatherData
{
    public int Id { get; set; }
    public DateTime? DateTime { get; set; }
    public double? Temperature { get; set; }
    public double? Humidity { get; set; }
    public double? DewPoint { get; set; }
    public double? Pressure { get; set; }
    public string? WindDirection { get; set; }
    public double? WindSpeed { get; set; }
    public int? Cloudiness { get; set; }
    public int? CloudinessLower { get; set; }
    public double? Visibility { get; set; }
    public string? WeatherPhenomenon { get; set; }
}