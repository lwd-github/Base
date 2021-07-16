using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace SignalRClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var enterpriseId = Console.ReadLine();

            HubConnection connection = new HubConnectionBuilder()
               .WithUrl("https://localhost:5801/msghub?access_token=a1", options =>
               {
                   options.AccessTokenProvider = () => Task.FromResult(enterpriseId);
               })
               .WithAutomaticReconnect()
               .Build();

            connection.On<string>("Self", cid =>
            {
                var newMessage = $"{cid}";

                Console.WriteLine($"From Service => {newMessage}");
            });

            connection.On<string, string>("ReceiveMsg", (user, message) =>
            {
                var newMessage = $"{user}: {message}";

                Console.WriteLine($"From Service => {newMessage}");
            });

            connection.On<string>("ReceiveGroupMsg", msg =>
            {
                Console.WriteLine($"From Service => {msg}");
            });

            await connection.StartAsync();

            await connection.InvokeAsync("SendMsg", "jack", "hello,world");

            Console.ReadLine();
        }
    }
}
