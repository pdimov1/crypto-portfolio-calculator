using CryptoPortfolioCalculator.API.Models;
using CryptoPortfolioCalculator.Application.Abstractions;
using CryptoPortfolioCalculator.Application.Services;
using Serilog;

namespace CryptoPortfolioCalculator.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console() // Logs to console
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // Logs to file daily
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpClient("CryptoCurrencyAPI", httpClient => httpClient.BaseAddress = new Uri(builder.Configuration.GetSection("ApiEndpoint")?.Value));

            builder.Services.AddAutoMapper(typeof(PortfolioItemProfile));

            builder.Services.AddScoped<IFileParserService, FileParserService>();
            builder.Services.AddScoped<IPortfolioCalculatorService, PortfolioCalculatorService>();
            builder.Services.AddScoped<ICryptoProviderService, CoinloreProviderService>();
            builder.Services.AddScoped<IFileValidationService, FileValidationService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("DefaultCorsPolicy", policy =>
                {
                    policy.WithOrigins("https://localhost:7113/")  // Replace with your actual domains
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("DefaultCorsPolicy");

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Append("Content-Security-Policy",
                    "default-src 'self'; " +
                    "script-src 'self' 'unsafe-inline'; " +
                    "style-src 'self' 'unsafe-inline'; " +
                    "connect-src 'self' ws: wss: http://localhost:* https://localhost:*;");

                await next();
            });

            app.UseAuthorization();

            app.MapControllers();

            app.UseMiddleware<ExceptionMiddleware>();

            app.Run();
        }
    }
}
