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
            var items = typeof(EOrderStatus).GetEnumItems();

            items.ForEach(item => {
                System.Console.WriteLine(item);
            });

            var status = EOrderStatus.等待付款;
            var nameValue = status.GetNameValue();
            var des = status.GetDescription();
            nameValue = new Order().Status.GetNameValue();
        }
    }


    public enum EOrderStatus
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

        public EOrderStatus? Status { get; set; }
    }
}
