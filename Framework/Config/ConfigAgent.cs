using Common.Extension;
using System.Collections.Generic;
using System.Linq;

namespace Config
{
    public abstract class ConfigAgent<T> where T : new()
    {
        /// <summary>
        /// 配置信息集
        /// </summary>
        static IEnumerable<ConfigItem> Configs;

        /// <summary>
        /// 加载配置信息
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<ConfigItem> LoadConfig();

        /// <summary>
        /// 配置值
        /// </summary>
        public T Value 
        {
            get { return GetValue(); }
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <returns></returns>
        T GetValue()
        {
            if (Configs.IsNull())
            {
                Configs = LoadConfig();
            }

            var name = typeof(T).Name.ToLower();
            var config = Configs.FirstOrDefault(p => p.Key.ToLower() == name);

            if (config.IsNotNull())
            {
                return config.Value.ToObject<T>();
            }

            return default(T);
        }
    }
}
