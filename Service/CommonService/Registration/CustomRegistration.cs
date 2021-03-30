using Autofac;
using Canal;
using Canal.Config;
using CommonService.Config;
using Config;
using IOC;
using MQ;
using MQ.Config;
using MQ.RabbitMQ;

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
                var content = new RabbitMQContext(ConfigAgent.Value<MQConfig>());
                return content;
            }).SingleInstance();

            //注册Canal客户端
            containerBuilder.Register<ICanalClient>(c =>
            {
                var content = new CanalClient(ConfigAgent.Value <CanalConfig>());
                return content;
            }).SingleInstance();
        }
    }
}
