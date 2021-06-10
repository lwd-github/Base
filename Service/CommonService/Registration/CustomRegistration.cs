using Autofac;
using Framework.Cache.Redis;
using Framework.Cache.Redis.Config;
using Framework.Canal;
using Framework.Canal.Config;
using Framework.Config;
using Framework.FTP;
using Framework.FTP.Config;
using Framework.MQ;
using Framework.MQ.Config;
using Framework.MQ.RabbitMQ;

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
            //注册Redis
            containerBuilder.Register<RedisCache>(c =>
            {
                var content = new RedisCache(ConfigAgent.Value<RedisConfig>());
                return content;
            }).SingleInstance();

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

            //注册Ftp客户端
            containerBuilder.Register<IFTPClient>(c =>
            {
                var content = new FTPClient(ConfigAgent.Value<FTPConfig>());
                return content;
            }).SingleInstance();
        }
    }
}
