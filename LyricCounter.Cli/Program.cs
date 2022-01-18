using System.Net.Http.Headers;
using CommandLine;
using LyricCounter.Cli.Api;
using LyricCounter.Cli.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Serilog;

namespace LyricCounter.Cli
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json").Build();
            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logfile.txt")
                .CreateLogger();

            var serviceProvider = BuildServices(configuration);
            
            var applicationEntryPoint = serviceProvider.GetRequiredService<LyricCounterApplication>();
        
            await Parser.Default.ParseArguments<CliStartupOptions>(args)
                .WithParsedAsync(async o => await applicationEntryPoint.RunApplicationAsync(o.ArtistName));
        }

        private static ServiceProvider BuildServices(IConfigurationRoot configuration)
        {
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(_ => configuration);
            services.AddSingleton<LyricCounterApplication>();
            services.AddHttpClient("Genius", config =>
            {
                config.BaseAddress = new Uri(configuration["LyricApiSettings:BaseUrl"]);
                config.DefaultRequestHeaders.Clear();
                config.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", configuration["LyricApiSettings:ClientAccessToken"]);
            }).AddTransientHttpErrorPolicy(poly => poly.WaitAndRetryAsync(
                new[]
                {
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10), 
                }));
            services.AddHttpClient("LyricsOvh", config =>
            {
                config.BaseAddress = new Uri(configuration["LyricsOvhApiSettings:BaseUrl"]);
            }).AddTransientHttpErrorPolicy(poly => poly.WaitAndRetryAsync(
                new[]
                {
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10), 
                }));
            services.AddTransient<IApi, Api.Api>();
            services.AddSingleton<IArtistRetriever, ArtistRetriever>();
            services.AddSingleton<ISongsRetriever,SongsRetriever>();
            services.AddSingleton<ILyricsRetriever, LyricsRetriever>();
            services.AddSingleton<IAverageLyricsCalculator, AverageLyricsCalculator>();
            services.AddSingleton<IConsoleOutput, ConsoleOutput>();
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}