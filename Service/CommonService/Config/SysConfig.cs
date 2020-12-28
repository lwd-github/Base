using Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonService.Config
{
    public class SysConfig<T> : ConfigAgent<T> where T : new()
    {
        //protected override IEnumerable<ConfigItem> ConfigItems => LoadConfigItems();

        static SysConfig()
        {
            ConfigItems = LoadConfigItems();
        }

        /// <summary>
        /// 加载配置信息
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<ConfigItem> LoadConfigItems()
        {
            return new List<ConfigItem> {
                new ConfigItem{
                    Key = "Auth",
                    Value = "{\"keySecrets\": [{\"key\": \"customersys001\",\"secret\": \"123456\"},{\"key\": \"javasystem\",\"secret\": \"807c93157b3e4f5f9cce582b65e369f8\"},{\"key\": \"pointssys\",\"secret\": \"02433D8EF45F4842A0330F4CF9E3BE35\"},{\"key\": \"zdwmssys\",\"secret\": \"b4122004eefe11e88c7f005056a939bd\"}]}"
                }
            };
    }
}
}
