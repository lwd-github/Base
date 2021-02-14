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
        private string[] _iocAssemblies;
        public AutofacServiceProviderFactory(string[] iocAssemblies)
        {
            _iocAssemblies = iocAssemblies;
        }

        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            IocManager.Init(builder, _iocAssemblies);
            builder.Populate(services);
            return builder;
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }
    }
}
