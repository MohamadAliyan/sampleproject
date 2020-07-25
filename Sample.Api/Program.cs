using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using System;

namespace Sample.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


            //var configuration = new ConfigurationBuilder()
            //    .AddJsonFile($"appsettings.{environment ?? "Production"}.json")
            //    .Build();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                //  .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(r => r.Exception != null && r.Exception.Message == "log")
                    .WriteTo.MSSqlServer("Data Source=.;Initial Catalog=Logs;integrated security=sspi;MultipleActiveResultSets=true", "MarketingLoginfos", restrictedToMinimumLevel: LogEventLevel.Information, autoCreateSqlTable: true))
                .CreateLogger();

            try
            {
                Log.Error("Starting Marketing Api host");
                Log.Information("Starting Marketing Api host");
                BuildWebHost(args).Run();
                // return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                // return;
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IWebHost BuildWebHost(string[] args) =>
            //WebHost.CreateDefaultBuilder(args)
            //    .UseStartup<Startup>()
            //    .Build();

            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseIISIntegration()
                .UseSerilog() // <-- Add this line
                .Build();
    }
}
