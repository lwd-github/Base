using Autofac;
using CommonService.Registration;
using CommonService.ServiceProviderFactory;
using Framework.Config;
using DTO.Constant;
using Framework.IOC;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    ConfigAgent.Configuration = hostingContext.Configuration;

                    ////注册Redis；在Startup.cs的方法public void ConfigureServices(IServiceCollection services)注册也可以
                    //services.AddSingleton(typeof(Cache.Redis.RedisCache), new Cache.Redis.RedisCache(Config.ConfigAgent.Value<Cache.Redis.Config.RedisConfig>()));
                })
                //方式一：需在Startup.cs 增加方法注册接口：public void ConfigureContainer(Autofac.ContainerBuilder builder)
                //.UseServiceProviderFactory(new AutofacServiceProviderFactory()); //指定 Autofac 工厂替换默认工厂

                //方式二
                .UseServiceProviderFactory(new CustomServiceProviderFactory());
    }
}
