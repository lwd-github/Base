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

                    ////ע��Redis����Startup.cs�ķ���public void ConfigureServices(IServiceCollection services)ע��Ҳ����
                    //services.AddSingleton(typeof(Cache.Redis.RedisCache), new Cache.Redis.RedisCache(Config.ConfigAgent.Value<Cache.Redis.Config.RedisConfig>()));
                })
                //��ʽһ������Startup.cs ���ӷ���ע��ӿڣ�public void ConfigureContainer(Autofac.ContainerBuilder builder)
                //.UseServiceProviderFactory(new AutofacServiceProviderFactory()); //ָ�� Autofac �����滻Ĭ�Ϲ���

                //��ʽ��
                .UseServiceProviderFactory(new CustomServiceProviderFactory());
    }
}
