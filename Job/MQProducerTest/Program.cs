using Autofac;
using Autofac.Extensions.DependencyInjection;
using CommonModel.Constant;
using IOC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace MQProducerTest
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
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddHostedService<MQProducerService>();
                })
                .UseServiceProviderFactory(new IOC.AutofacServiceProviderFactory(MQConstant.IOCAssemblies.Split(';')))
                .Build();
        }

    }
}
