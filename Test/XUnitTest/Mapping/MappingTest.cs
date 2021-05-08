using System;
using System.Collections.Generic;
using Xunit;
using Framework.Common.Extension;
using System.Diagnostics;
using XUnitTest.Model;
using XUnitTest.Enum;

namespace XUnitTest.Map
{
    public class MappingTest
    {
        /// <summary>
        /// 测试对象的转换
        /// </summary>
        [Fact]
        public void Test()
        {
            int count = 50000;
            Student student;
            List<Person> persons = new List<Person>();

            for (int i = 1; i <= count; i++)
            {
                persons.Add(
                    new Person
                    {
                        Id = i,
                        Name = $"Name_{i}",
                        Birthday = i <= 12 ? new DateTime(1995, i, 1) : new DateTime(1995, 1, 18),
                        Sex = i > 5 ? ESex.男 : ESex.女,
                        Attribute = new Dictionary<string, int> { { i.ToString(), i + 1 } },
                        Test = new List<int> { i, i + 1, i + 2 }
                    });
            }

            var sw = Stopwatch.StartNew();
            persons.ForEach(person =>
            {
                student = person.Map<Student>();
            });
            sw.Stop();
            //2万条转换耗时0.697秒
            //3万条转换耗时0.863秒
            //4万条转换耗时1.078秒
            //5万条转换耗时1.116秒
            var msg = $"转换数：{count}，耗时：{sw.Elapsed.TotalSeconds}秒";

            student = new Student
            {
                Id = "11",
                Name = $"Name_{11}",
                Birthday = "1995-11-1",
                Sex = ESex.未知,
                Attribute = new Dictionary<string, int> { { "1", 8 } },
                Test = new List<int> { 0, 1, 2 },
                AdmissionDate = new DateTime(2000,9,1)
            };

            var person = student.Map<Person>();
        }


        /// <summary>
        /// 创建动态对象
        /// </summary>
        [Fact]
        public void Test1()
        {
            //var items = new List<string>();
            //items.Add($"\"Id\":\"111\"");
            //items.Add($"\"name\":\"张三\"");
            //var json = $"{{{string.Join(",", items)}}}";
            //var obj = json.ToObject<Person>();

            //var abc = new ABC<string> { Id = 2, Obj = json };
            //json = abc.ToJson();
            //var obj1 = json.ToObject<ABC<Person>>();


            Dictionary<string, object> temp = new Dictionary<string, object>();
            temp["id"] = 31;
            temp["Name"] = "测试";
            temp["Birthday"] = DateTime.Now;

            dynamic obj = new System.Dynamic.ExpandoObject();

            foreach (KeyValuePair<string, object> item in temp)
            {
                ((IDictionary<string, object>)obj).Add(item.Key, item.Value);
            }

            var json = ((object)obj).ToJson();
            var json1 = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            //var abc = new ABC<string> { Id = 2, CreateTime = DateTime.Now, Obj = json };
            var abc = new ABC<object> { Id = 2, CreateTime = DateTime.Now, Obj = obj };
            json = abc.ToJson();
            var obj1 = json.ToObject<ABC<Person>>();
        }
    }

    public class ABC<T>
    {
        public int Id { get; set; }

        public DateTime CreateTime { get; set; }

        public T Obj { get; set;}
    }
}
