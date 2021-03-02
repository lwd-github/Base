using Cache.Local;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTest.Cache
{
    public class CacheTest
    {
        [Fact]
        public void Test()
        {
            string key = "cache_key1";

            ILocalCache localCache = new LocalCache();
            var i = localCache.GetOrSet<int?>(key, () => { return 2; });

            var j = localCache.Get<int?>(key);
            localCache.Remove(key);
            j = localCache.Get<int?>(key);
        }
    }
}
