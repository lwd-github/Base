using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignalRClientController : ControllerBase
    {
        [HttpPost]
        public async void Post(string groupId, string msg)
        {
            HubConnection connection = new HubConnectionBuilder()
               .WithUrl("https://localhost:5801/msghub", options =>
               {
                   options.AccessTokenProvider = () => Task.FromResult(groupId);
               })
               .WithAutomaticReconnect()
               .Build();

            await connection.StartAsync();
            await connection.InvokeAsync("SendGroupMsg", groupId, msg);
        }
    }
}
