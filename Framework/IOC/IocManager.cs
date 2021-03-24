using Autofac;
using Autofac.Features.AttributeFilters;
using Common.Extension;
using IOC.LifetimeScope;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Loader;

namespace IOC
{
    public class IocManager
    {
        private static ContainerBuilder _containerBuilder;
        private static IContainer _container;

        /// <summary>
        /// Ioc初始化
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="assemblyNames"></param>
        public static void Init(ContainerBuilder containerBuilder, params string[] assemblyNames)
        {
            _containerBuilder = containerBuilder;
            RegisterAssemblyTypes(containerBuilder, assemblyNames); //注册程序集类型
            //_container = builder.Build();
        }


        /// <summary>
        /// 设置容器
        /// </summary>
        /// <param name="container"></param>
        public static void SetContainer(IContainer container)
        {
            _container = container;
        }


        /// <summary>
        /// 获取容器
        /// </summary>
        /// <returns></returns>
        public static IContainer GetContainer()
        {
            return _container;
        }


        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementer"></typeparam>
        /// <param name="serviceName"></param>
        public static void RegisterType<TService, TImplementer>(string serviceName = null)
        {
            if (serviceName.IsNullOrWhiteSpace())
            {
                _containerBuilder.RegisterType(typeof(TImplementer)).As<TService>();
            }
            else
            {
                _containerBuilder.RegisterType(typeof(TImplementer)).Named<TService>(serviceName);
            }
        }


        /// <summary>
        /// 从容器检索服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TService Resolve<TService>()
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                return scope.Resolve<TService>();
            }
        }


        /// <summary>
        /// 从容器检索服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        public static TService Resolve<TService>(string serviceName) where TService : class
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                return scope.ResolveOptionalNamed<TService>(serviceName);
            }
        }


        /// <summary>
        /// 注册程序集类型
        /// </summary>
        /// <param name="containerBuilder">容器生成器</param>
        /// <param name="assemblyNames">程序集</param>
        private static void RegisterAssemblyTypes(ContainerBuilder containerBuilder, params string[] assemblyNames)
        {
            //预先加载dll
            Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
            .Where(t => assemblyNames.Contains(Path.GetFileName(t)))
            .ToList().ForEach(file => AssemblyLoadContext.Default.LoadFromAssemblyPath(file));

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(t => !t.IsDynamic && assemblyNames.Contains(Path.GetFileName(t.Location)))
                .ToArray();

            containerBuilder.RegisterAssemblyTypes(assemblies)
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .WithAttributeFiltering();

            //处理下面的个性化接口
            containerBuilder.RegisterAssemblyTypes(assemblies)
                .As<ISingleInstance>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance()
                .WithAttributeFiltering();

            containerBuilder.RegisterAssemblyTypes(assemblies)
                //.Except<IDependencySingleton>()
                //.Except<IDependencyRequestSingleton>()
                .As<IInstancePerDependency>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .WithAttributeFiltering();

            containerBuilder.RegisterAssemblyTypes(assemblies)
                .As<IInstancePerLifetimeScope>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .WithAttributeFiltering();
        }
    }
}
