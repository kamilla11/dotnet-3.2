using Grpc.Core;
using Grpc.Net.Client;
using Jwt;
using SecretInfo;

using var channel = GrpcChannel.ForAddress("http://localhost:5141");

var client = new JwtService.JwtServiceClient(channel);

var serverData = client.GetJwt(new JWTRequest(){Username = "Kamilla"});

var token = serverData.Token;

if (token is null) throw new Exception("token is null");

var client2 = new SecretService.SecretServiceClient(channel);

var secret = client2.GetSecret(new SecretInfo.SecretRequest(), headers: new Metadata { { "Authorization", $"Bearer {token}" } });

Console.WriteLine($"Please, keep your secret: {secret.Secret}");