using Autofac;
using CommonModel.Constant;
using CommonService.Config;
using CommonService.Registration;
using MQ;
using MQ.Config;
using MQ.RabbitMQ;
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
    public class CustomServiceProviderFactory : IOC.ServiceProviderFactory
    {
        public CustomServiceProviderFactory() : base(MQConstant.IOCAssemblies.Split(';'))
        {
            //自定义接口注册
            CustomRegistration.Register(ContainerBuilder);
        }
    }
}
