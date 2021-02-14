using Autofac;
using Autofac.Features.AttributeFilters;
using IOC.LifetimeScope;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Loader;

namespace IOC
{
    public class IocManager
    {
        public static void Init(ContainerBuilder builder, params string[] assemblyNames)
        {
            //预先加载dll
            Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
            .Where(t => assemblyNames.Contains(Path.GetFileName(t)))
            .ToList().ForEach(file => AssemblyLoadContext.Default.LoadFromAssemblyPath(file));

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(t => assemblyNames.Contains(Path.GetFileName(t.Location)))
                .ToArray();

            builder.RegisterAssemblyTypes(assemblies)
                .As<ISingleInstance>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance()
                .WithAttributeFiltering();

            builder.RegisterAssemblyTypes(assemblies)
                //.Except<IDependencySingleton>()
                //.Except<IDependencyRequestSingleton>()
                .As<IInstancePerDependency>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .WithAttributeFiltering();

            builder.RegisterAssemblyTypes(assemblies)
                .As<IInstancePerLifetimeScope>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .WithAttributeFiltering();
        }
    }
}
