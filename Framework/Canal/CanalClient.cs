using Canal.Config;
using CanalSharp.Client;
using CanalSharp.Client.Impl;
using System;
using System.Threading;

namespace Canal
{
    public class CanalClient : ICanalClient
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


        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="action"></param>
        public void Receive(Action<CanalMessage<string>> action)
        {
            while (true)
            {
                //获取数据 1024表示数据大小 单位为字节
                var message = _connector.Get(1024);

                //批次id 可用于回滚
                var batchId = message.Id;
                if (batchId == -1 || message.Entries.Count <= 0)
                {
                    Thread.Sleep(300);
                    continue;
                }

                var canalMessage = new CanalMessage<string>();
                action(canalMessage);
            }
        }
    }
}
