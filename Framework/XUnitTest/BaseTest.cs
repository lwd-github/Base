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
    public class BaseTest : IDisposable
    {
        private static IContainer _container;
        protected ILifetimeScope LifetimeScope;

        static BaseTest()
        {
            var factory = new ServiceProviderFactory(MQConstant.IOCAssemblies.Split(';'));
            var containerBuilder = factory.CreateBuilder(new ServiceCollection());
            CustomRegistration.Register(containerBuilder);//自定义接口注册
            _container = containerBuilder.Build();
        }


        public BaseTest()
        {
            LifetimeScope = _container.BeginLifetimeScope();
        }


        public void Dispose()
        {
            _container?.Dispose();
        }
    }
}
