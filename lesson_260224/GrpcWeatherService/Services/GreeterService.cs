// using Grpc.Core;
// using GrpcWatherService;
// using GrpcWeatherService;
//
// namespace GrpcWatherService.Services;
//
// public class GreeterService : Greeter.GreeterBase
// {
//     private readonly ILogger<GreeterService> _logger;
//
//     public GreeterService(ILogger<GreeterService> logger)
//     {
//         _logger = logger;
//     }
//
//     public override async Task<WeatherReply> GetTemperature(WeatherRequest request, ServerCallContext context)
//     {
//         using (var client = new HttpClient())
//         {
//             using var result = await client.GetFromJsonAsync<OpenMeteoResult>($"https://api.open-meteo.com/v1/forecast?latitude=55.7887&longitude=49.1221&hourly=temperature_2m&timezone=Europe%2FMoscow&forecast_hours={request}");
//             return new WeatherReply()
//             {
//                 Message = result.hourly.time.Last() + " " + result.hourly.temperature_2m.Last()
//             };
//         }
//         
//       
//     }
// }