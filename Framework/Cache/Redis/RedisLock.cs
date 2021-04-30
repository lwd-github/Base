using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cache.Redis
{
    public class RedisLock : IDisposable
    {
        Func<IDatabase> _getDatabase;
        string _key; //锁键
        string _token; //令牌
        bool _isReleased; //是否释放锁

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="database">Redis 数据库对象</param>
        /// <param name="key">锁键</param>
        public RedisLock(Func<IDatabase> getDatabase, string key)
        {
            _getDatabase = getDatabase;
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
            return Do(db =>
            {
                ulong expirationNew = expiration > 0 ? expiration : ulong.MaxValue;
                return db.LockTake(_key, _token, TimeSpan.FromSeconds(expirationNew));
            });
        }


        /// <summary>
        /// 释放锁
        /// </summary>
        /// <returns></returns>
        public bool LockRelease()
        {
            return Do(db =>
            {
                _isReleased = db.LockRelease(_key, _token);
                return _isReleased;
            });
        }


        public void Dispose()
        {
            if (!_isReleased)
            {
                //释放锁
                Do(db => db.LockRelease(_key, _token));
            }
        }


        private T Do<T>(Func<IDatabase, T> func)
        {
            var database = _getDatabase();
            return func(database);
        }
    }
}
