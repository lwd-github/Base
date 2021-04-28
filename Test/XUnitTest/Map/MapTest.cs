using Map;
using System;
using System.Collections.Generic;
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

        [Fact]
        public void GetLocationByGaoDe()
        {
            var result = MapHelper.GetLocationByGaoDe("10b17eb8743753de4e04f784c87c8761", new List<string> {"abc","乌鲁木齐", "广州市", "广东省广州市天河区车陂高地大街南1号", "广东省广州市天河区银汇大厦", "广东省汕尾区城区" });
        }
    }
}
