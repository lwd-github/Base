using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using IdGenerator;
using System.Collections.Concurrent;

namespace XUnitTest.IdGenerator
{
    public class IdGeneratorTest : BaseTest
    {
        ConcurrentDictionary<long, long> dict = new ConcurrentDictionary<long, long>();
        IdWorker idWorker = new IdWorker(1, 1);

        [Fact]
        public void Test()
        {
            var id = idWorker.NextId();

            Parallel.Invoke(
                () => Method1(),
                () => Method2(),
                () => Method3()
                );
        }

        private void Method1()
        {
            for (int i = 0; i < 2000; i++)
            {
                var id = idWorker.NextId();
                dict.TryAdd(id, id);
            }
            
        }

        private void Method2()
        {
            for (int i = 0; i < 2000; i++)
            {
                var id = idWorker.NextId();
                dict.TryAdd(id, id);
            }

        }


        private void Method3()
        {
            for (int i = 0; i < 2000; i++)
            {
                var id = idWorker.NextId();
                dict.TryAdd(id, id);
            }

        }
    }
}
