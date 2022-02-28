using Framework.Common.Number;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTest.Number
{
    public class NumberTest
    {
        [Fact]
        public void Test()
        {
            long d0 = 190;
            Decimal1 d1 = 1.234M;
            Decimal2 d2 = 1.345M;

            var r1 = d0 + d1 + d2;
            Decimal2 r2 = d0 + d1 + d2;

            var r3 = d1 > d2 ? d1 : d2;
        }
    }
}
