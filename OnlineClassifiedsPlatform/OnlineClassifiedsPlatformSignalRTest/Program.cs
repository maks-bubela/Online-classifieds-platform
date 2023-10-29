using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using System;

namespace OnlineClassifiedsPlatformSignalRTest
{
    class Program
    {
        static void Main(string[] args)
        {

            var connection = new HubConnectionBuilder().WithUrl("https://localhost:44380/goods").Build();

            connection.On<SignalRMessage>("newMessage", (message) =>
            {
                Console.WriteLine(message.Arguments);
            });

            connection.On("Start", (string server, string message) =>
            {
                Console.WriteLine($"Message from server {server}: {message}");
            }
            );

            connection.On<string>("SendNoticeEventToClient", param => {
                Console.WriteLine(param);
            });
            connection.StartAsync();
            connection.InvokeAsync("Start").Wait();
            Console.ReadKey();
        }
    }
}
