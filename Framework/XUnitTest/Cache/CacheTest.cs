using Autofac;
using Cache.Local;
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
            _localCache = IocManager.Resolve<ILocalCache>();
        }


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
    }
}
