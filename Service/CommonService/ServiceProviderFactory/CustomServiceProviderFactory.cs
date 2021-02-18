using Autofac;
using CommonModel.Constant;
using CommonService.Config;
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
    /// 定制的接口注册
    /// </summary>
    public class CustomServiceProviderFactory : IOC.AutofacServiceProviderFactory
    {
        public CustomServiceProviderFactory() : base(MQConstant.IOCAssemblies.Split(';'))
        {
            //注册MQ
            ContainerBuilder.Register<IMQContext>(c =>
            {
                var content = new RabbitMQContext(new SysConfig().Value<MQConfig>());
                return content;
            }).SingleInstance();
        }
    }
}
