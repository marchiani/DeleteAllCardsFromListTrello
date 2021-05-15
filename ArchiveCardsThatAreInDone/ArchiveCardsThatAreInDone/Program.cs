using System;
using System.Reflection;
using ArchiveCardsThatAreInDone.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ArchiveCardsThatAreInDone
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureServices);
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(ArchiveCardsService));

            services.AddHostedService<App>();
        }
    }
}
