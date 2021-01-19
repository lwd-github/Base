using Autofac;
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
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .Build();
        }

        class AutofacServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
        {
            public ContainerBuilder CreateBuilder(IServiceCollection services)
            {
                throw new NotImplementedException();
            }

            public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 新增该方法：用于IOC注册程序集类型
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(Autofac.ContainerBuilder builder)
        {
            IocManager.Init(builder, MQConstant.IOCAssemblies.Split(';'));
        }
    }
}
