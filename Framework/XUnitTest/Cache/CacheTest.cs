using Autofac;
using Cache.Local;
using Cache.Redis;
using Config;
using IOC;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using XUnitTest.Model;
using Common.Extension;
using System.Threading;
using System.Threading.Tasks;

namespace XUnitTest.Cache
{
    public class CacheTest : BaseTest
    {
        readonly ILocalCache _localCache;
        readonly RedisCache _redisCache;

        public CacheTest()
        {
            var connectionString = ConfigAgent.Configuration["DB:ConnectionString"];
            _localCache = IocManager.Resolve<ILocalCache>();
            _redisCache = IocManager.Resolve<RedisCache>();
        }


        /// <summary>
        /// 本地缓存测试
        /// </summary>
        [Fact]
        public void LocalCacheTest()
        {
            string key1 = "localCache_key1";

            //ILocalCache localCache = new LocalCache();
            var i = _localCache.GetOrSet<int?>(key1, () => { return 2; });

            var j = _localCache.Get<int?>(key1);
            _localCache.Remove(key1);
            j = _localCache.Get<int?>(key1);

            string key2 = "localCache_key2";
            var i2 = _localCache.Get<ETest?>(key2);
        }


        /// <summary>
        /// Redis缓存测试
        /// </summary>
        [Fact]
        public void RedisCacheTest()
        {
            string key1 = "redisCache_key1";

            //ILocalCache localCache = new LocalCache();
            var i = _redisCache.GetOrSet<int?>(key1, () => { return 2; });

            var j = _redisCache.Get<int?>(key1);
            _redisCache.Remove(key1);
            j = _redisCache.Get<int?>(key1);

            var k = _redisCache.GetOrSet<string>(key1, () => { return "8"; }, 30);
            k = _redisCache.Get<string>(key1);

            string key2 = "redisCache_key2";
            _redisCache.Remove(key2);
            var i2 = _redisCache.GetOrSet<ETest?>(key2, () => { return ETest.失败; });
            i2 = _redisCache.Get<ETest?>(key2);

            string key3 = "redisCache_key3";
            _redisCache.Remove(key3);
            var i3 = _redisCache.GetOrSet(key3, () => { return new Person { Id = 1, Name = "Test" }; });
            i3 = _redisCache.Get<Person>(key3);

            string key4 = "redisCache_key4";
            _redisCache.Remove(key4);
            var i4 = _redisCache.GetOrSet<Location?>(key4, () => { return new Location { Lat = 1.2, Lng = 2.3 }; });
            i4 = _redisCache.Get<Location?>(key4);
        }


        [Fact]
        public void RedisLockTest()
        {
            Parallel.Invoke(
                () => LockRun1(),
                () => LockRun2(),
                () => LockRun3()
                );
        }


        private void LockRun1()
        {
            using (var redisLock = _redisCache.CreateLock("lock1"))
            {
                if (redisLock.LockTake())
                {
                    //doing
                    Thread.Sleep(30 * 1000);
                }
                else
                {
                    Console.WriteLine("获取锁失败");
                }
            } 
        }


        private void LockRun2()
        {
            using (var redisLock = _redisCache.CreateLock("lock1"))
            {
                if (redisLock.LockTake())
                {
                    //doing
                    Thread.Sleep(30 * 1000);
                }
                else
                {
                    Console.WriteLine("获取锁失败");
                }

                redisLock.LockRelease();
            }
        }


        private void LockRun3()
        {
            var redisLock = _redisCache.CreateLock("lock1");

            if (redisLock.LockTake())
            {
                //doing
                Thread.Sleep(30 * 1000);
            }
            else
            {
                Console.WriteLine("获取锁失败");
            }

            redisLock.LockRelease();
        }


        /// <summary>
        /// 测试类型转换
        /// </summary>
        [Fact]
        public void ChangeType()
        {
            var result = Get<string>();
            var result1 = Get<int>();
            var result2 = Get<ETest>();
        }


        [Fact]
        public void ToObject()
        {
            var s = ETest.失败;
            var type = s.GetType();
            var isSerializable = type.IsSerializable;

            if((type.IsValueType || type.IsClass) && type != typeof(string))
            {
                var js = s.ToJson();
                var sO = js.ToObject<ETest>();
            }
        }


        public T Get<T>()
        {
            T result = default(T);

            Type type = typeof(T);
            bool isClass = type.IsValueType;

            if (type == typeof(string))
            {
                result = (T)Convert.ChangeType("1", typeof(T));
            }
            return result;
        }
    }

    public enum ETest
    {
        成功 = 0,
        失败 = 1
    }
}
