using CryptoPortfolioCalculator.Clients.Abstractions;
using CryptoPortfolioCalculator.Clients.Clients;
using CryptoPortfolioCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CryptoPortfolioCalculator
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

            // Add services to the container.
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            builder.Services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
                options.Cookie.Name = "CSRF-TOKEN";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });

            builder.Services.AddAutoMapper(typeof(PortfolioProfile));

            builder.Services.Configure<PortfolioSettings>(builder.Configuration.GetSection("PortfolioSettings"));

            builder.Services.AddHttpClient("PortfolioAPI", httpClient => httpClient.BaseAddress = new Uri(builder.Configuration.GetSection("PortfolioSettings:ApiEndpoint")?.Value));

            builder.Services.AddScoped<IFileClient, FileClient>();
            builder.Services.AddScoped<IPortfolioClient, PortfolioClient>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseMiddleware<ExceptionMiddleware>();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Append("Content-Security-Policy",
                    "default-src 'self'; " +
                    "script-src 'self' 'unsafe-inline'; " +
                    "style-src 'self' 'unsafe-inline'; " +
                    "connect-src 'self' ws: wss: http://localhost:* https://localhost:*;");

                await next();
            });

            app.Run();
        }
    }
}
