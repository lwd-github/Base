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
            //获取枚举成员信息
            var items = typeof(OrderStatus).GetItems();

            items.ForEach(item => {
                System.Console.WriteLine(item);
            });

            var des = new Order().Id.GetType().GetDescription();

            var status = OrderStatus.等待付款;
            
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

    public class Order
    {
        [Description("订单唯一标识")]
        public string Id { get; set; }
    }
}
