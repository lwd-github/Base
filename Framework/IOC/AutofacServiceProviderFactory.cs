using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOC
{
    public class AutofacServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        protected ContainerBuilder ContainerBuilder;
        private string[] _iocAssemblies;

        public AutofacServiceProviderFactory(string[] iocAssemblies)
        {
            ContainerBuilder = new ContainerBuilder();
            _iocAssemblies = iocAssemblies;
        }

        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            //ContainerBuilder = new ContainerBuilder();
            IocManager.Init(ContainerBuilder, _iocAssemblies);
            ContainerBuilder.Populate(services);
            return ContainerBuilder;
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }
    }
}
