﻿using CommonService.ServiceProviderFactory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace MQConsumerTest
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
                    services.AddHostedService<MQConsumerService>();
                })
                .UseServiceProviderFactory(new CustomServiceProviderFactory())
                .Build();
        }
    }
}
