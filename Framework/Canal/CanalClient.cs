using Canal.Config;
using CanalSharp.Client;
using CanalSharp.Client.Impl;
using CanalSharp.Protocol;
using Com.Alibaba.Otter.Canal.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Canal
{
    public class CanalClient : ICanalClient, IDisposable
    {
        private ICanalConnector _connector;

        public CanalClient(CanalConfig config)
        {
            //创建一个简单CanalClient连接对象（此对象不支持集群）传入参数分别为 canal地址、端口、destination、用户名、密码
            //canal的默认端口：11111
            _connector = CanalConnectors.NewSingleConnector(config.Host, config.Port, config.Destination, config.User, config.Password);

            //连接 Canal
            _connector.Connect();

            //订阅，Filter是一种过滤规则，通过该规则的表数据变更才会传递过来
            //允许所有数据 .*\\..*
            //允许某个库数据 库名\\..*
            //允许某些表 库名.表名,库名.表名
            //多个规则组合使用：canal\\..*,mysql.test1,mysql.test2 (逗号分隔)；注意：此过滤条件只针对row模式的数据有效
            _connector.Subscribe(config.Subscribe);
        }

        public void Dispose()
        {
            _connector?.Disconnect();
        }


        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="action"></param>
        public void Receive(Action<List<CanalMessage>> action)
        {
            while (true)
            {
                //批次id 可用于回滚
                long batchId = -1;

                try
                {
                    //获取数据 1024表示数据大小 单位为字节
                    var message = _connector.Get(1024);

                    batchId = message.Id;

                    if (batchId == -1 || message.Entries.Count <= 0)
                    {
                        Thread.Sleep(300);
                        continue;
                    }

                    var canalMessage = BuildMessages(message);
                    action(canalMessage);
                }
                catch (Exception)
                {
                    if (batchId != -1)
                    {
                        _connector.Rollback(batchId);
                    }
                }
            }
        }


        /// <summary>
        /// 生成Canal消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private List<CanalMessage> BuildMessages(Message message)
        {
            var canalMessages = new List<CanalMessage>();
            var entries = message.Entries;
            var now = DateTime.Now;

            foreach (var entry in entries)
            {
                if (entry.EntryType == EntryType.Transactionbegin || entry.EntryType == EntryType.Transactionend)
                {
                    continue;
                }

                RowChange rowChange = null;

                try
                {
                    rowChange = RowChange.Parser.ParseFrom(entry.StoreValue);
                }
                catch (Exception e)
                {
                    throw;
                    //_logger.LogError(e.ToString());
                }

                if (rowChange != null)
                {
                    EventType eventType = rowChange.EventType;
                    CanalMessage canalMessage = new CanalMessage();

                    //_logger.LogInformation(
                    //    $"================> binlog[{entry.Header.LogfileName}:{entry.Header.LogfileOffset}] , name[{entry.Header.SchemaName},{entry.Header.TableName}] , eventType :{eventType}");

                    canalMessage.Id = message.Id;
                    canalMessage.SchemaName = entry.Header.SchemaName;
                    canalMessage.TableName = entry.Header.TableName;

                    foreach (var rowData in rowChange.RowDatas)
                    {
                        if (eventType == EventType.Delete)
                        {
                            canalMessage.Action = EActionType.Delete;
                            canalMessage.Before = BuildDataObject(rowData.BeforeColumns.ToList());
                        }
                        else if (eventType == EventType.Insert)
                        {
                            canalMessage.Action = EActionType.Insert;
                            canalMessage.After = BuildDataObject(rowData.AfterColumns.ToList());
                        }
                        else
                        {
                            canalMessage.Action = EActionType.Update;
                            canalMessage.Before = BuildDataObject(rowData.BeforeColumns.ToList());
                            canalMessage.After = BuildDataObject(rowData.AfterColumns.ToList());
                        }
                    }

                    canalMessages.Add(canalMessage);
                }
            }

            return canalMessages;
        }


        /// <summary>
        /// 生成数据对象
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        private object BuildDataObject(List<Column> columns)
        {
            //var items = new List<string>();

            //foreach (var column in columns)
            //{
            //    //Console.WriteLine($"{column.Name} ： {column.Value}  update=  {column.Updated}");
            //    items.Add($"\"{column.Name}\":\"{column.Value}\"");
            //}

            //return $"{{{string.Join(",", items)}}}";

            dynamic obj = new System.Dynamic.ExpandoObject();

            foreach (var column in columns)
            {
                ((IDictionary<string, object>)obj).Add(column.Name, column.Value);
            }

            return obj;
        }
    }
}
