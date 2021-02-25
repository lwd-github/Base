using Autofac;
using CommonService.Config;
using MQ;
using MQ.Config;
using MQ.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonService.Registration
{
    /// <summary>
    /// 自定义接口注册类
    /// </summary>
    public class CustomRegistration
    {
        /// <summary>
        /// 接口注册
        /// </summary>
        /// <param name="containerBuilder"></param>
        public static void Register(ContainerBuilder containerBuilder)
        {
            //注册MQ
            containerBuilder.Register<IMQContext>(c =>
            {
                var content = new RabbitMQContext(new SysConfig().Value<MQConfig>());
                return content;
            }).SingleInstance();
        }
    }
}
