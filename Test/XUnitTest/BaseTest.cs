using Autofac;
using DTO.Constant;
using CommonService.Registration;
using CommonService.ServiceProviderFactory;
using Framework.IOC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Framework.Config;
using Microsoft.Extensions.Configuration.Json;

namespace XUnitTest
{
    public class BaseTest
    {
        static BaseTest()
        {
            //配置文件
            var configuration = new ConfigurationBuilder()
                    .Add(new JsonConfigurationSource { Path = $"appsettings.json", ReloadOnChange = true })
                    .Build();
            ConfigAgent.Configuration = configuration;

            //IOC注册
            var containerBuilder = new ContainerBuilder();
            IocManager.Init(containerBuilder, MQConstant.IOCAssemblies.Split(';'));
            CustomRegistration.Register(containerBuilder);//自定义接口注册
            IocManager.SetContainer(containerBuilder.Build());
        }
    }
}
