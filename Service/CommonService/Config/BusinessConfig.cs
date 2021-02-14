using Config;
using Common.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOC.LifetimeScope;

namespace CommonService.Config
{
    public class BusinessConfig: ConfigAgent, ISingleInstance
    {
        /// <summary>
        /// 加载配置信息
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<ConfigItem> LoadConfig()
        {
            return new List<ConfigItem>
            {
                new ConfigItem
                {
                    Key = "WMS",
                    Value = "{\"Url\":\"https://wms.com\",\"Security\":{\"PublicKey\":\"keyab123\"}}"
                }
            };
        }
    }
}
