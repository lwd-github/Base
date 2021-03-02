using Common.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cache
{
    public abstract class CacheBase : ICache
    {
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <returns></returns>
        public string Get(string key)
        {
            return Get<string>(key);
        }


        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">缓存值类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <returns></returns>
        public abstract T Get<T>(string key);


        /// <summary>
        /// 获取缓存，如果为Null时，执行委托方法，并将委托返回的结果写入缓存
        /// </summary>
        /// <typeparam name="T">缓存值类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <param name="func">委托方法</param>
        /// <param name="expiration">缓存有效期（单位：秒）</param>
        /// <returns></returns>
        public T GetOrSet<T>(string key, Func<T> func, uint expiration = 0)
        {
            var value = Get<T>(key);

            if (value.IsNull())
            {
                value = func();
                Set<T>(key, value, expiration);
            }

            return value;
        }


        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public abstract void Remove(string key);


        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值</param>
        /// <param name="second">缓存有效期（单位：秒）</param>
        public void Set(string key, string value, uint expiration = 0)
        {
            Set<string>(key, value, expiration);
        }


        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">缓存值类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值</param>
        /// <param name="expiration">缓存有效期（单位：秒）</param>
        public abstract void Set<T>(string key, T value, uint expiration = 0);
    }
}
