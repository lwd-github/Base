using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOC
{
    public class ServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        //private ContainerBuilder _containerBuilder;
        private string[] _iocAssemblies;
        private Action<ContainerBuilder> _action;

        public ServiceProviderFactory(string[] iocAssemblies, Action<ContainerBuilder> action = null)
        {
            _iocAssemblies = iocAssemblies;
            _action = action;
            //_containerBuilder = new ContainerBuilder();
            //ContainerBuilder = new ContainerBuilder();
            //IocManager.Init(ContainerBuilder, _iocAssemblies);
        }

        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var containerBuilder = new ContainerBuilder();
            IocManager.Init(containerBuilder, _iocAssemblies);
            _action?.Invoke(containerBuilder);
            containerBuilder.Populate(services);
            return containerBuilder;
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            IocManager.SetContainer(containerBuilder.Build());
            return new AutofacServiceProvider(IocManager.GetContainer());
        }
    }
}
