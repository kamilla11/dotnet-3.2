namespace GrpcWeatherService.Services;

public interface IWeatherData
{
    public Task<OpenMeteoResult> GetWeatherAsync();
}

public class WeatherData: IWeatherData
{
    private static OpenMeteoResult? data;
    public async Task<OpenMeteoResult> GetWeatherAsync()
    {
        if (data is not null)
            return data;

        using var client = new HttpClient();
        {
            var result = await client.GetFromJsonAsync<OpenMeteoResult>("https://api.open-meteo.com/v1/forecast?latitude=55.7887&longitude=49.1221&hourly=temperature_2m&timezone=Europe%2FMoscow&past_days=92");
            return result;
        }
    }
}