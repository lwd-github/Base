using Common.Extension;
using System.Collections.Generic;
using System.Linq;

namespace Config
{
    public abstract class ConfigAgent
    {
        ///// <summary>
        ///// 配置信息集
        ///// </summary>
        //static IEnumerable<ConfigItem> _configs;

        /// <summary>
        /// 加载配置信息
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<ConfigItem> LoadConfig();

        /// <summary>
        /// 配置值
        /// </summary>
        public T Value<T>() where T : new()
        {
            //if (_configs.IsNull())
            //{
            //    _configs = LoadConfig();
            //}

            var name = typeof(T).Name.ToLower();
            //var config = _configs.FirstOrDefault(p => p.Key.ToLower() == name);
            var config = LoadConfig().FirstOrDefault(p => p.Key.ToLower() == name);

            if (config.IsNotNull())
            {
                return config.Value.ToObject<T>();
            }

            return default;
        }
    }
}
