using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cache.Redis
{
    public class RedisLock : IDisposable
    {
        IDatabase _database;
        string _key; //锁键
        string _token; //令牌
        bool _isReleased; //是否释放锁

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="database">Redis 数据库对象</param>
        /// <param name="key">锁键</param>
        public RedisLock(IDatabase database, string key)
        {
            _database = database;
            _key = key;
            _token = Guid.NewGuid().ToString();
        }


        /// <summary>
        /// 获取锁
        /// </summary>
        /// <param name="expiration">锁有效期（单位：秒），默认60秒</param>
        /// <returns></returns>
        public bool LockTake(ulong expiration = 60)
        {
            ulong expirationNew = expiration > 0 ? expiration : ulong.MaxValue;
            return _database.LockTake(_key, _token, TimeSpan.FromSeconds(expirationNew));
        }


        /// <summary>
        /// 释放锁
        /// </summary>
        /// <returns></returns>
        public bool LockRelease()
        {
            return _isReleased = _database.LockRelease(_key, _token);
        }


        public void Dispose()
        {
            if(!_isReleased)
            {
                _database.LockRelease(_key, _token); //释放锁
            }
        }
    }
}
