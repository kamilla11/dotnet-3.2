using GrpcWeatherService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Services.AddSingleton<IWeatherData, WeatherData>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.RequireHttpsMetadata = false;
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "issuer",
            ValidAudience = "audience",
            IssuerSigningKey =
                new SymmetricSecurityKey("6D4955F3-5AB1-444F-8DF3-E982C7DC5C3A6D4955F3-5AB1-444F-8DF3-E982C7DC5C3A"u8.ToArray())
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapGrpcService<WeatherService>();
app.MapGrpcService<JwtService>();
app.MapGrpcService<SecretService>();

app.Run();