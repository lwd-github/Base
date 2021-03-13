using Map;
using System;
using Xunit;

namespace XUnitTest.Map
{
    public class MapTest
    {
        [Fact]
        public void Test()
        {
            var result = MapHelper.GetDistance(36.981149, 107.948021, 41.245658, 115.357613);
            Assert.True(result == 796336.97869444778);
        }
    }
}
