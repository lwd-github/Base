using Autofac;
using DTO.Constant;
using CommonService.Config;
using CommonService.Registration;
using Framework.MQ;
using Framework.MQ.Config;
using Framework.MQ.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonService.ServiceProviderFactory
{
    /// <summary>
    /// 自定义服务工厂
    /// </summary>
    public class CustomServiceProviderFactory : Framework.IOC.ServiceProviderFactory
    {
        public CustomServiceProviderFactory() : base(MQConstant.IOCAssemblies.Split(';'), CustomRegistration.Register)
        {
            //自定义接口注册
            //CustomRegistration.Register(ContainerBuilder);
        }
    }
}
