using Common.Extension;
using System.Collections.Generic;
using System.Linq;

namespace Config
{
    public abstract class ConfigAgent<T> where T: new()
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        protected static IEnumerable<ConfigItem> ConfigItems;

        ///// <summary>
        ///// 加载配置信息
        ///// </summary>
        ///// <returns></returns>
        //public abstract IEnumerable<ConfigItem> LoadConfigItem();

        /// <summary>
        /// 配置信息实例
        /// </summary>
        public static T Instance { get; private set; }

        static ConfigAgent()
        {
            if (Instance.IsNull())
            {
                var name = typeof(T).Name.ToLower();
                var config = ConfigItems.FirstOrDefault(p => p.Key == name);

                if(config.IsNotNull())
                {
                    Instance = config.Value.ToObject<T>();
                }
            }
        }
    }
}
