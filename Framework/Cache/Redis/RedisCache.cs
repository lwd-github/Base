using Cache.Local;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using Common.Extension;
using Cache.Redis.Config;

namespace Cache.Redis
{
    public class RedisCache : CacheBase, ILocalCache
    {
        /// <summary>
        /// Redis的数据库序号（默认16个，0~15），但做集群使用时只认 db0
        /// </summary>
        private int _dbNum { get; set; }

        /// <summary>
        /// Redis连接
        /// </summary>
        private readonly ConnectionMultiplexer _conn;

        public RedisHash Hash { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">Redis配置</param>
        /// <param name="dbNum">Redis的数据库序号</param>
        public RedisCache(RedisConfig config, int dbNum = 0)
        {
            _dbNum = dbNum;
            _conn = RedisConnection.GetConnectionMultiplexer($"{config.Host}:{config.Port}");
            Hash = new RedisHash(GetDatabase());
        }


        // <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">缓存值类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <returns></returns>
        public override T Get<T>(string key)
        {
            return Do(db =>
            {

                Type type = typeof(T).GetUnderlyingType();
                var value = db.StringGet(key).ToString();

                if (value.IsNull())
                {
                    return default;
                }
                else if (type == typeof(string))
                {
                    //if (type.IsEnum)
                    //{
                    //    return (T)Enum.Parse(type, value);
                    //}
                    //else
                    //{
                    //    return (T)Convert.ChangeType(value, type);
                    //}
                    return (T)Convert.ChangeType(value, type);
                }
                else
                {
                    return value.ToString().ToObject<T>();
                }
            });
        }


        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        public override void Remove(string key)
        {
            Do(db => db.KeyDelete(key));
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
            var result = Do(db =>
            {

                Type type = typeof(T).GetUnderlyingType();
                var valueFormat = type != typeof(string) ? value.ToJson() : value.ToString();

                return expiration > 0 ? db.StringSet(key, valueFormat, TimeSpan.FromSeconds(expiration)) : db.StringSet(key, valueFormat);
            });

            if (!result)
            {
                throw new RedisException($"设置键值失败，键名：{key}");
            }
        }


        /// <summary>
        /// 设置缓存的有效期
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="expiration">缓存有效期（单位：秒）</param>
        /// <returns></returns>
        public bool KeyExpire(string key, ulong expiration)
        {
            return Do(db => db.KeyExpire(key, TimeSpan.FromSeconds(expiration)));
        }


        /// <summary>
        /// 创建锁
        /// </summary>
        /// <param name="key">锁键</param>
        /// <returns></returns>
        public RedisLock CreateLock(string key)
        {
            var database = GetDatabase();
            return new RedisLock(database, key);
        }


        private T Do<T>(Func<IDatabase, T> func)
        {
            var database = GetDatabase();
            return func(database);
        }


        /// <summary>
        /// 获取Redis数据库对象
        /// </summary>
        /// <returns></returns>
        private IDatabase GetDatabase()
        { 
            return _conn.GetDatabase(_dbNum);
        }
    }
}
