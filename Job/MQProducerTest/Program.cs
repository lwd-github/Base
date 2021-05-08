using Framework.Common.Extension;
using CommonService.ServiceProviderFactory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Framework.Config;
using Microsoft.Extensions.Configuration;

namespace Job.MQProducerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = BuildWebHost(args);
            builder.Run();
        }

        static IHost BuildWebHost(string[] args)
        {
            return new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    if (env.EnvironmentName.IsNotNullOrEmpty() && env.EnvironmentName != Environments.Production)
                        config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

                })
                .ConfigureServices((hostingContext, services) =>
                {
                    ConfigAgent.Configuration = hostingContext.Configuration;
                    services.AddHostedService<MQProducerService>();
                })
                .UseServiceProviderFactory(new CustomServiceProviderFactory())
                .Build();
        }

    }
}
