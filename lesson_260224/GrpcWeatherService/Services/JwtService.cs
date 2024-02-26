using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Grpc.Core;
using Jwt;
using Microsoft.IdentityModel.Tokens;

namespace GrpcWeatherService.Services;

public class JwtService: Jwt.JwtService.JwtServiceBase
{
    public override Task<JWTResponse> GetJwt(JWTRequest request, ServerCallContext context)
    {
        var jwt = GenerateJwtTokenAsync(request.Username);
        return Task.FromResult(new JWTResponse { Token = jwt });
    }
    public string? GenerateJwtTokenAsync(string username)
    {
        var now = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(
            issuer: "issuer",
            audience: "audience",
            notBefore: now,
            claims: GetIdentity(username).Claims,
            expires: now + TimeSpan.FromMinutes(5),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey("6D4955F3-5AB1-444F-8DF3-E982C7DC5C3A6D4955F3-5AB1-444F-8DF3-E982C7DC5C3A"u8.ToArray()),
                SecurityAlgorithms.HmacSha256));
    
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
    
    private static ClaimsIdentity GetIdentity(string username)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, username),
        };
        var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
    }
}