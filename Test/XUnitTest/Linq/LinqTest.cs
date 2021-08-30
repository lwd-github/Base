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
        List<Record> records = new List<Record>()
        {
            new Record { ProductId = 1, Qty = 100}, //入库
            new Record { ProductId = 1, Qty = -20}, //出库
            new Record { ProductId = 1, Qty = 10 }, //入库
            new Record { ProductId = 2, Qty = 50 }, //入库
            new Record { ProductId = 2, Qty = -5 }, //出库
            new Record { ProductId = 3, Qty = 30 }, //入库
        };

        List<Product> products = new List<Product>
        {
            new Product { Id =1, Name ="商品1" },
            new Product { Id =2, Name ="商品2" },
            new Product { Id =3, Name ="商品3" }
        };

        [Fact]
        public void AggregateTest()
        {
            var result = records.GroupBy(t => t.ProductId).Select(g => new Record
            {
                ProductId = g.Key,
                Qty = g.Sum(t => t.Qty)
            });

            //获取最小
            var min = result.Aggregate((a, b) => a.Qty <= b.Qty ? a : b);

            //
            var dtos = result.Join(products, record => record.ProductId, product => product.Id,
                (record, product) =>
                {
                    return new RecordDto
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Qty = record.Qty
                    };
                });

            products.AddRange(new List<Product>
            {
                new Product { Id =1, Name ="商品1-1" },
                new Product { Id =2, Name ="商品2-1" },
                new Product { Id =3, Name ="商品3-1" }
            });
            products = products.Distinct(new ProductComparer()).ToList();
        }
    }

    public class Record
    {
        public int ProductId { get; set; }

        public decimal Qty { get; set; }
    }

    public class RecordDto : Record
    {
        public string ProductName { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class ProductComparer : IEqualityComparer<Product>
    {
        bool IEqualityComparer<Product>.Equals(Product x, Product y)
        {
            return x.Id == y.Id;
        }

        int IEqualityComparer<Product>.GetHashCode(Product obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
