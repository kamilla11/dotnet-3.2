using System.Text.Json.Serialization;

namespace GrpcWeatherService;

public class HourlyData
{
    public List<string> Time { get; set; } = default!;
    [JsonPropertyName("temperature_2m")]
    public List<double> Temperature { get; set; } = default!;
}

public class HourlyUnits
{
    public string Time { get; set; } = default!;
    [JsonPropertyName("temperature_2m")]
    public string Temperature { get; set; }= default!;
}

public class OpenMeteoResult
{
    [JsonPropertyName("hourly_units")]
    public HourlyUnits HourlyUnits { get; set; } = default!;
    public HourlyData Hourly { get; set; }= default!;
}
