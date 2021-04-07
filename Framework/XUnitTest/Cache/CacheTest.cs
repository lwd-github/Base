using Autofac;
using Cache.Local;
using Config;
using IOC;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTest.Cache
{
    public class CacheTest: BaseTest
    {
        readonly ILocalCache _localCache;

        public CacheTest()
        {
            var connectionString = ConfigAgent.Configuration["DB:ConnectionString"];
            _localCache = IocManager.Resolve<ILocalCache>();
        }


        /// <summary>
        /// 测试缓存
        /// </summary>
        [Fact]
        public void Test()
        {
            string key = "cache_key1";

            //ILocalCache localCache = new LocalCache();
            var i = _localCache.GetOrSet<int?>(key, () => { return 2; });

            var j = _localCache.Get<int?>(key);
            _localCache.Remove(key);
            j = _localCache.Get<int?>(key);
        }


        /// <summary>
        /// 测试类型转换
        /// </summary>
        [Fact]
        public void ChangeType()
        {
            var result = Get<string>();
        }


        public T Get<T>()
        {
            T result = default(T);
               
            if(typeof(T).FullName == "System.String")
            {
                result = (T)Convert.ChangeType("1", typeof(T));
            }
            return result;
        }
    }
}
