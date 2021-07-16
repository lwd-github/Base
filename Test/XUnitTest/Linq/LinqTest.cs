using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTest.Linq
{
    public class LinqTest
    {
        [Fact]
        public void AggregateTest()
        {
            var records = new List<Record>();

            records.AddRange(new List<Record>() {
                new Record { ProductId = 1, Qty = 100}, //入库
                new Record { ProductId = 1, Qty = -20}, //出库
                new Record { ProductId = 1, Qty = 10}, //入库
                new Record { ProductId = 2, Qty = 50}, //入库
                new Record { ProductId = 2, Qty = -5}, //出库
                new Record { ProductId = 3, Qty = 30}, //入库
            });

            var result = records.GroupBy(t => t.ProductId).Select(g => new Record
            {
                ProductId = g.Key,
                Qty = g.Sum(t => t.Qty)
            });

            var min = result.Aggregate((a, b) => a.Qty <= b.Qty ? a : b);
        }
    }

    public class Record
    {
        public int ProductId { get; set; }

        public decimal Qty { get; set; }
    }
}
