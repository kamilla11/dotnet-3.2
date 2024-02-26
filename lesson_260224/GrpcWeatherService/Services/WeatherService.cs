using Grpc.Core;
using Weather;

namespace GrpcWeatherService.Services;

public class WeatherService: Weather.WeatherService.WeatherServiceBase
{
    private IWeatherData _weatherData;

    public WeatherService(IWeatherData weatherData)
    {
        _weatherData = weatherData;
    }
    public override async Task WeatherStream(WeatherRequest request, IServerStreamWriter<WeatherResponse> responseStream, ServerCallContext context)
    {
        var data = await _weatherData.GetWeatherAsync();
        
        var i = 0;
        while (!context.CancellationToken.IsCancellationRequested)
        {
            var now = DateTime.Now.ToString("HH:mm:ss");
            var weatherDate = DateTime.Parse(data.Hourly.Time[i]).ToString("dd.MM.yyy H:mm");
            var temperature = data.Hourly.Temperature[i] + data.HourlyUnits.Temperature;
            await responseStream.WriteAsync(new WeatherResponse { Data = $"{now} погода на {weatherDate} {temperature}" });
            await Task.Delay(TimeSpan.FromSeconds(1));
            i=i+2;
            if (i > data.Hourly.Time.Count - 1) break;
        }
    }
}