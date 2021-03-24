using Autofac;
using CommonService.Registration;
using DTO.Constant;
using IOC;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace TestWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();
            var host = CreateHostBuilder(args, containerBuilder).Build();
            CustomRegistration.Register(containerBuilder);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, ContainerBuilder containerBuilder) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                //方式一：需在Startup.cs 增加方法注册接口：public void ConfigureContainer(Autofac.ContainerBuilder builder)
                //.UseServiceProviderFactory(new AutofacServiceProviderFactory()); //指定 Autofac 工厂替换默认工厂

                //方式二
                .UseServiceProviderFactory(new ServiceProviderFactory(containerBuilder, MQConstant.IOCAssemblies.Split(';')));
    }
}
