using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using SongScraping.Infrastructure.Persistence;

namespace MusicGames.Seeding
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
                        configHost.AddEnvironmentVariables(prefix: "SONGSCRAPE_");
                        configHost.AddCommandLine(args);
                    })
                    .ConfigureServices(serviceCollection =>
                    {
                        serviceCollection.AddScoped<ISeeding, TestSeeder>();
                        serviceCollection.AddDbContext<GameContext>(
                            options => options
                                .UseSqlite(@"Data Source=streamer-site.db",
                                    b=>b.MigrationsAssembly("MusicGames.Seeding"))
                        );
                        serviceCollection.AddDbContext<Ez2OnGameTrackContext>(
                            options => options
                                .UseSqlite(@"Data Source=streamer-site.db",
                                    b=>b.MigrationsAssembly("MusicGames.Seeding"))
                            );
                    })
                    .UseSerilog()
                ;
        }
    }
}