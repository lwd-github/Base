using Common.Extension;
using System.ComponentModel;
using Xunit;

namespace XUnitTest.Enum
{
    public class EnumTest
    {
        [Fact]
        public void GetEnumItems()
        {
            var items = typeof(OrderStatus).GetItems();

            items.ForEach(item => {
                System.Console.WriteLine(item);
            });
        }
    }


    public enum OrderStatus
    { 
        [Description("订单等待付款")]
        等待付款 = 0,
        待发货 = 1,
        待收货 = 2,
        已收货 = 3
    }
}
