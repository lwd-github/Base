using Framework.Config;
using Framework.Common.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.IOC.LifetimeScope;
using Framework.Cache.Local;
using DTO.Constant;

namespace CommonService.Config
{
    //public class BusinessConfig: ConfigAgent, ISingleInstance
    //{
    //    private ILocalCache _localCache;

    //    public BusinessConfig(ILocalCache localCache)
    //    {
    //        _localCache = localCache;
    //    }


    //    /// <summary>
    //    /// 获取配置信息
    //    /// </summary>
    //    /// <returns></returns>
    //    protected override IEnumerable<ConfigItem> GetConfig()
    //    {
    //        return _localCache.GetOrSet(CacheKeys.BusinessConfigKey, GetConfigFromDB, 30 * 60);
    //    }


    //    private IEnumerable<ConfigItem> GetConfigFromDB()
    //    {
    //        return new List<ConfigItem>
    //        {
    //            new ConfigItem
    //            {
    //                Key = "WMS",
    //                Value = "{\"Url\":\"https://wms.com\",\"Security\":{\"PublicKey\":\"keyab123\"}}"
    //            }
    //        };
    //    }
    //}
}
