using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace SignalRClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HubConnection connection = new HubConnectionBuilder()
               .WithUrl("https://localhost:5001/msghub", options =>
               {
                   options.AccessTokenProvider = () => Task.FromResult("abc123");
               })
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

            await connection.StartAsync();

            await connection.InvokeAsync("SendMsg", "jack", "hello,world");

            Console.ReadLine();
        }
    }
}
