using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicCatalog.EFCore.Persistence;
using Serilog;

namespace MusicCatalog.Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            try
            {
                Log.Information("Args: {Args}", args);
                
                var host = CreateHostBuilder(args).Build();
                using (var serviceScope = host.Services.CreateScope())
                {
                    var services = serviceScope.ServiceProvider;
                    var seeder = services.GetRequiredService<ISeeding>();
                    seeder.Seed();
                }
                
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "an error has occured");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                    .ConfigureHostConfiguration(configHost =>
                    {
                        configHost.SetBasePath(Directory.GetCurrentDirectory());
                        configHost.AddEnvironmentVariables(prefix: "EZ2CATALOG_");
                        configHost.AddCommandLine(args);
                    })
                    .ConfigureServices(
                        (hostContext, services) =>
                        {
                            // Set the active provider via configuration
                            var configuration = hostContext.Configuration;
                            var provider = configuration.GetValue("Provider", "SqlServer");
                            var seedMode = configuration.GetValue("SeedMode", "Migrate-Only");
                            switch (seedMode)
                            {
                                case "Migrate-Only":
                                    throw new NotImplementedException("Migration-Only Seeder not implemented");
                        
                                case "Seed-Test":
                                    services.AddScoped<ISeeding, TestSeeder>();
                                    break;
                                default:
                                    throw new Exception("Unrecognized Seed Mode");
                            }
                            
                            services.AddDbContext<GameContext>(
                                options => _ = provider switch
                                {
                                    "Sqlite" => options.UseSqlite(
                                        configuration.GetConnectionString("SqliteConnection"),
                                        x => x.MigrationsAssembly("MusicCatalog.Migrations")),
                                    
                                    "SqlServer" => options.UseSqlServer(
                                        configuration.GetConnectionString("SqlServerConnection"),
                                        x => x.MigrationsAssembly("MusicCatalog.Migrations")),
                                    
                                    _ => throw new Exception($"Unsupported provider: {provider}")
                                });
                        })
                    .UseSerilog()
                ;
        }
    }
}