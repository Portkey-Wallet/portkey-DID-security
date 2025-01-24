using CASecurity.Silo;
using CASecurity.Silo.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CASecurity;
public class Program
{
    public async static Task<int> Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        
        try
        {
            Log.Information("Starting CASecurity.Silo.");

            await CreateHostBuilder(args).RunConsoleAsync();

            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
            Console.WriteLine("Host terminated unexpectedly!");
            Console.WriteLine(ex);
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine(ex.Message);
            Console.WriteLine("Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    internal static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseApolloForHostBuilder()
            .ConfigureServices((hostcontext, services) =>
            {
                services.AddApplication<CASecurityOrleansSiloModule>();
            })
            .UseOrleansSnapshot()
            .UseAutofac()
            .UseSerilog();
    
}