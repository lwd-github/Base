using CommonService.ServiceProviderFactory;
using Framework.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AggregationApi
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
                .UseServiceProviderFactory(new CustomServiceProviderFactory());
    }
}
