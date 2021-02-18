using CanalSharp.Client.Impl;
using Com.Alibaba.Otter.Canal.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CanalTest
{
    class Program
    {
        //canal客户端测试
        static void Main(string[] args)
        {
            //canal 配置的 destination，默认为 example
            var destination = "example";
            //创建一个简单CanalClient连接对象（此对象不支持集群）传入参数分别为 canal地址、端口、destination、用户名、密码
            //canal的默认端口：11111
            var connector = CanalConnectors.NewSingleConnector("127.0.0.1", 11111, destination, "", "");
            //连接 Canal
            connector.Connect();
            //订阅，同时传入 Filter。Filter是一种过滤规则，通过该规则的表数据变更才会传递过来
            //允许所有数据 .*\\..*
            //允许某个库数据 库名\\..*
            //允许某些表 库名.表名,库名.表名
            //多个规则组合使用：canal\\..*,mysql.test1,mysql.test2 (逗号分隔)；注意：此过滤条件只针对row模式的数据有效
            connector.Subscribe(".*\\..*");

            while (true)
            {
                //获取数据 1024表示数据大小 单位为字节
                var message = connector.Get(1024);
                //批次id 可用于回滚
                var batchId = message.Id;
                if (batchId == -1 || message.Entries.Count <= 0)
                {
                    Thread.Sleep(300);
                    continue;
                }

                PrintEntry(message.Entries);
            }
        }


        /// <summary>
        /// 输出数据
        /// </summary>
        /// <param name="entrys">一个entry表示一个数据库变更</param>
        private static void PrintEntry(List<Entry> entrys)
        {
            foreach (var entry in entrys)
            {
                if (entry.EntryType == EntryType.Transactionbegin || entry.EntryType == EntryType.Transactionend)
                {
                    continue;
                }

                RowChange rowChange = null;

                try
                {
                    //获取行变更
                    rowChange = RowChange.Parser.ParseFrom(entry.StoreValue);
                }
                catch (Exception e)
                {

                }

                if (rowChange != null)
                {
                    //变更类型 insert/update/delete 等等
                    EventType eventType = rowChange.EventType;
                    //输出binlog信息 表名 数据库名 变更类型
                    Console.WriteLine(
                        $"================> binlog[{entry.Header.LogfileName}:{entry.Header.LogfileOffset}] , name[{entry.Header.SchemaName},{entry.Header.TableName}] , eventType :{eventType}");

                    //输出 insert/update/delete 变更类型列数据
                    foreach (var rowData in rowChange.RowDatas)
                    {
                        if (eventType == EventType.Delete)
                        {
                            PrintColumn(rowData.BeforeColumns.ToList());
                        }
                        else if (eventType == EventType.Insert)
                        {
                            PrintColumn(rowData.AfterColumns.ToList());
                        }
                        else
                        {
                            Console.WriteLine("-------> before");
                            PrintColumn(rowData.BeforeColumns.ToList());
                            Console.WriteLine("-------> after");
                            PrintColumn(rowData.AfterColumns.ToList());
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 输出每个列的详细数据
        /// </summary>
        /// <param name="columns"></param>
        private static void PrintColumn(List<Column> columns)
        {
            foreach (var column in columns)
            {
                //输出列明 列值 是否变更
                Console.WriteLine($"{column.Name} ： {column.Value}  update=  {column.Updated}");
            }
        }

    }
}
