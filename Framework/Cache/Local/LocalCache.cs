using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;

namespace Cache.Local
{
    /// <summary>
    /// 本地缓存
    /// </summary>
    public class LocalCache : CacheBase, ILocalCache
    {
        static readonly MemoryCache _cache = new MemoryCache(Options.Create(new MemoryCacheOptions()));

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">缓存值类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <returns></returns>
        public override T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }


        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public override void Remove(string key)
        {
            _cache.Remove(key);
        }


        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">缓存值类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值</param>
        /// <param name="expiration">缓存有效期（单位：秒）</param>
        public override void Set<T>(string key, T value, ulong expiration = 0)
        {
            if (expiration > 0)
            {
                _cache.Set<T>(key, value, DateTimeOffset.Now.AddSeconds(expiration));
            }
            else
            {
                _cache.Set<T>(key, value);
            }
        }
    }
}
