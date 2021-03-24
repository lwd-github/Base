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
                //��ʽһ������Startup.cs ���ӷ���ע��ӿڣ�public void ConfigureContainer(Autofac.ContainerBuilder builder)
                //.UseServiceProviderFactory(new AutofacServiceProviderFactory()); //ָ�� Autofac �����滻Ĭ�Ϲ���

                //��ʽ��
                .UseServiceProviderFactory(new ServiceProviderFactory(containerBuilder, MQConstant.IOCAssemblies.Split(';')));
    }
}
