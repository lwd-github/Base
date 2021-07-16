using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRService
{
    public class MessageHub : Hub
    {
        IHttpContextAccessor _httpContextAccessor;
        ////一个链接加入多个分组
        //ConcurrentDictionary<string, List<string>> _enterpriseGroupMapping = new ConcurrentDictionary<string, List<string>>();
        static ConcurrentDictionary<string, string> _enterpriseGroupMapping = new ConcurrentDictionary<string, string>();

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
            var accessToken = _httpContextAccessor.HttpContext.Request.Query["access_token"].ToString();
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["authorization"].ToString();
            accessToken = accessToken.Replace("bearer", string.Empty, true, null).Trim();

            var cid = Context.ConnectionId;
            Console.WriteLine($"[{cid}]连接成功");

            string groupName = accessToken;
            _enterpriseGroupMapping.TryAdd(cid, groupName);

            //根据id获取指定客户端
            var client = Clients.Client(cid);

            //向指定用户发送消息
            await client.SendAsync("Self", cid);

            ////像所有用户发送消息
            //await Clients.All.SendAsync("AddMsg", $"{cid}加入了聊天室");

            await Groups.AddToGroupAsync(cid, groupName); //将链接加入组
            await SendGroupMsg(groupName, $"{cid} has joined the group {groupName}.");
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

            string groupName;
            var isSuccess = _enterpriseGroupMapping.TryGetValue(cid, out groupName);

            if(isSuccess)
            {
                await Groups.RemoveFromGroupAsync(cid, groupName); //将链接移除组
                await SendGroupMsg(groupName, $"{cid} has joined the group {groupName}.");
            }
        }


        public async Task SendMsg(string user, string message)
        {
            Console.WriteLine($"From Client => {user}:{message}");
            await Clients.All.SendAsync("ReceiveMsg", user, message);
        }

        public async Task SendGroupMsg(string groupId, string msg)
        {
            await Clients.Group(groupId).SendAsync("ReceiveGroupMsg", msg);
        }
    }
}
