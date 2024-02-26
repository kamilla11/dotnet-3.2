using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using SecretInfo;

namespace GrpcWeatherService.Services;

[Authorize]
public class SecretService: SecretInfo.SecretService.SecretServiceBase
{
    public override Task<SecretResponse> GetSecret(SecretRequest request, ServerCallContext context)
    {
        return Task.FromResult(new SecretResponse { Secret = "42" });
    }
}