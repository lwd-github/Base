using Framework.Common.Extension;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Config
{
    public class ConfigAgent
    {
        ///// <summary>
        ///// 配置信息集
        ///// </summary>
        //static IEnumerable<ConfigItem> _configs;

        ///// <summary>
        ///// 获取配置信息
        ///// </summary>
        ///// <returns></returns>
        //protected abstract IEnumerable<ConfigItem> GetConfig();


        /// <summary>
        /// 配置文件
        /// </summary>
        /// Nuget安装Microsoft.Extensions.Configuration
        public static IConfiguration Configuration { get; set; }


        static IEnumerable<ConfigItem> GetConfig()
        {
            var connectionString = Configuration["DB:ConnectionString"];

            return new List<ConfigItem>
            {
                new ConfigItem
                {
                    Key = "Auth",
                    Value = "{\"keySecrets\": [{\"key\": \"customersys001\",\"secret\": \"123456\"},{\"key\": \"javasystem\",\"secret\": \"807c93157b3e4f5f9cce582b65e369f8\"},{\"key\": \"pointssys\",\"secret\": \"02433D8EF45F4842A0330F4CF9E3BE35\"},{\"key\": \"zdwmssys\",\"secret\": \"b4122004eefe11e88c7f005056a939bd\"}]}"
                },
                //new ConfigItem
                //{
                //    Key ="MQConfig",
                //    Value = "{\"host\": \"119.147.171.113\",\"port\": \"5672\",\"user\": \"zhidian\",\"password\": \"zhidian\",\"virtualHost\": \"/\"}"
                //},
                //new ConfigItem
                //{
                //    Key ="MQConfig",
                //    Value = "{\"host\": \"127.0.0.1\",\"port\": \"5673\",\"user\": \"guest\",\"password\": \"guest\",\"virtualHost\": \"/\"}"
                //}
                //new ConfigItem
                //{
                //    Key ="RedisConfig",
                //    Value = "{\"host\": \"127.0.0.1\",\"port\": \"6379\"}"
                //},
                new ConfigItem
                {
                    Key ="RedisConfig",
                    Value = "{\"host\": \"47.242.83.236\",\"port\": \"6379\",\"password\": \"redis@123qaz\"}"
                },
                new ConfigItem
                {
                    Key ="MQConfig",
                    Value = "{\"host\": \"47.242.83.236\",\"port\": \"5672\",\"user\": \"admin\",\"password\": \"admin@689\",\"virtualHost\": \"/\"}"
                },
                new ConfigItem
                {
                    Key ="CanalConfig",
                    Value = "{\"host\": \"127.0.0.1\",\"port\": \"11111\",\"user\": \"\",\"password\": \"\",\"Subscribe\": \".*\\\\..*\"}"
                },
                new ConfigItem
                {
                    Key ="FtpConfig",
                    Value = "{\"host\": \"47.242.83.236\",\"port\": \"21\",\"user\": \"lwd\",\"password\": \"lwd123\"}"
                },
            };
        }


        /// <summary>
        /// 配置值
        /// </summary>
        public static T Value<T>() where T : new()
        {
            //if (_configs.IsNull())
            //{
            //    _configs = LoadConfig();
            //}

            var name = typeof(T).Name.ToLower();
            //var config = _configs.FirstOrDefault(p => p.Key.ToLower() == name);
            var config = GetConfig().FirstOrDefault(p => p.Key.ToLower() == name);

            if (config.IsNotNull())
            {
                return config.Value.ToObject<T>();
            }

            return default;
        }
    }
}
