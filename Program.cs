using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Exceptions;
using System;

namespace RefactorThis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = GetLogger();
            try
            {
                Log.Information("Starting web application");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();


        private static ILogger GetLogger()
        {
            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console()
                .WriteTo.File("./logs/log-.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit:true, fileSizeLimitBytes: 10485760)
                .MinimumLevel.Information();

            return loggerConfiguration.CreateLogger();
        }

    }
}