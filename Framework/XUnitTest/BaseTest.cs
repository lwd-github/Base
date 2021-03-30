using Autofac;
using DTO.Constant;
using CommonService.Registration;
using CommonService.ServiceProviderFactory;
using IOC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTest
{
    public class BaseTest
    {
        static BaseTest()
        {
            var containerBuilder = new ContainerBuilder();
            IocManager.Init(containerBuilder, MQConstant.IOCAssemblies.Split(';'));
            CustomRegistration.Register(containerBuilder);//自定义接口注册
            IocManager.SetContainer(containerBuilder.Build());
        }
    }
}
