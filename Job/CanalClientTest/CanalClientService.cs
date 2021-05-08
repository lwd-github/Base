using Framework.Canal;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Framework.Common.Extension;
using Framework.MQ;
using Framework.MQ.Config;
using RabbitMQ.Client;

namespace Job.CanalClientTest
{
    public class CanalClientService : IHostedService
    {
        readonly ICanalClient _canalClient;
        readonly IMQContext _mqContext;

        public CanalClientService(ICanalClient canalClient, IMQContext mqContext)
        {
            _canalClient = canalClient;
            _mqContext = mqContext;
        }

        private void Start()
        {
            _canalClient.Receive(Receive);
        }

        private void Receive(List<CanalMessage> msgs)
        {
            msgs.ForEach(msg =>
                {
                    Console.WriteLine($"接收到Canal消息：{msg.ToJson()}");

                    //发布/订阅（广播）
                    var exchangeName = $"Canal_{msg.TableName}_Exchange";
                    var producer = _mqContext.CreateProducer(new Exchange { Name = exchangeName, Type = ExchangeType.Fanout });
                    producer.Send(msg.ToJson());
                }
            );
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(Start);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
