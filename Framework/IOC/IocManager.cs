using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.AttributeFilters;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Loader;

namespace IOC
{
    public class IocManager
    {
        //readonly static IContainer _container;
        //readonly static ContainerBuilder _builder;

        //static IocManager()
        //{
        //    //_builder = new ContainerBuilder();
        //    //_container = _builder.Build();
        //}

        public static void Init(ContainerBuilder builder, params string[] assemblyNames)
        {
            //预先加载dll
            var a = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
            .Where(t => assemblyNames.Contains(Path.GetFileName(t)));
            //.Each(file => AssemblyLoadContext.Default.LoadFromAssemblyPath(file));

            foreach (var item in a)
            {
                AssemblyLoadContext.Default.LoadFromAssemblyPath(item);

            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(t => assemblyNames.Contains(Path.GetFileName(t.Location)))
                .ToArray();

            builder.RegisterAssemblyTypes(assemblies)
                .As<ISingleInstance>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance()
                .WithAttributeFiltering();

        }

        //public static IServiceProvider CreateAutofacServiceProvider()
        //{
        //    return new AutofacServiceProvider(_container);
        //}
    }
}
