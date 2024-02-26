using Grpc.Core;
using Grpc.Net.Client;
using Weather;

using var channel = GrpcChannel.ForAddress("http://localhost:5141");

var client = new WeatherService.WeatherServiceClient(channel);

var serverData = client.WeatherStream(new WeatherRequest());

var responseStream = serverData.ResponseStream;

var cancellationTokenSource = new CancellationTokenSource();

Task.Run(() =>
{
    Thread.Sleep(TimeSpan.FromSeconds(20));
    cancellationTokenSource.Cancel();
});

try
{
    await foreach(var response in responseStream.ReadAllAsync(cancellationTokenSource.Token))
    {
        Console.WriteLine(response.Data);
    }
}
catch (Exception)
{
    Console.WriteLine("the end");
}