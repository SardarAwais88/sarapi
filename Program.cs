using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sarapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               //.WriteTo.File(path: "E:\\Sarapi\\logs\\log -.txt",

                // this path is use when we publish our api to live server
               .WriteTo.File(path: "\\logs\\log -.txt",
               outputTemplate: "{TimeStamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}]{Message:lj}{NewLine}{Exception}",
               rollingInterval: RollingInterval.Day,
               restrictedToMinimumLevel: LogEventLevel.Information
               ).CreateLogger();

            // If any Error Occour use try catch block to handle authorization
            try
            {
                Log.Information("Application is starting");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {

                Log.Fatal(e, "Application failed to start");
            }
            {
                Log.CloseAndFlush();
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                 //   webBuilder.UseStartup<Startup>();
                    webBuilder.UseStartup<Startup>();
                });
    }
}
