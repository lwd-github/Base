using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRService
{
    public class MessageHub : Hub
    {
        IHttpContextAccessor _httpContextAccessor;

        public MessageHub(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 客户连接成功时触发
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var token = _httpContextAccessor.HttpContext.Request.Query["access_token"];

            var cid = Context.ConnectionId;
            Console.WriteLine($"[{cid}]连接成功");

            //根据id获取指定客户端
            var client = Clients.Client(cid);

            //向指定用户发送消息
            await client.SendAsync("Self", cid);

            ////像所有用户发送消息
            //await Clients.All.SendAsync("AddMsg", $"{cid}加入了聊天室");
        }


        /// <summary>
        /// 客户端断开连接时触发
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var cid = Context.ConnectionId;
            Console.WriteLine($"[{cid}]断开连接");
        }


        public async Task SendMsg(string user, string message)
        {
            Console.WriteLine($"From Client => {user}:{message}");
            await Clients.All.SendAsync("ReceiveMsg", user, message);
        }
    }
}
