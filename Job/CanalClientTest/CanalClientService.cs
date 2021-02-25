using Canal;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CanalClientTest
{
    public class CanalClientService : IHostedService
    {
        readonly ICanalClient _canalClient;

        public CanalClientService(ICanalClient canalClient)
        {
            _canalClient = canalClient;
        }

        private void Start()
        {
            _canalClient.Receive(Receive);
        }

        private void Receive(CanalMessage<string> msg)
        {
            Console.WriteLine($"接收到Canal消息：{msg}");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(Start);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
