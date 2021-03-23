﻿using Config;
using Common.Extension;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOC.LifetimeScope;
using Cache.Local;
using DTO.Constant;

namespace CommonService.Config
{
    public class SysConfig : ConfigAgent, ISingleInstance
    {
        private ILocalCache _localCache;

        public SysConfig(ILocalCache localCache)
        {
            _localCache = localCache;
        }


        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<ConfigItem> GetConfig()
        {
            return _localCache.GetOrSet(CacheKeys.SysConfigKey, GetConfigFromDB, 30 * 60);
        }


        private IEnumerable<ConfigItem> GetConfigFromDB()
        {
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
            };
        }
    }
}
