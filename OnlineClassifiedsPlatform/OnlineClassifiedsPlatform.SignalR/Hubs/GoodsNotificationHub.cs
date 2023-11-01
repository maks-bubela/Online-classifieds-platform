using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.ServiceBus;
using OnlineClassifiedsPlatform.Configuration;
using Newtonsoft.Json;
using OnlineClassifiedsPlatform.AzureServiceBus.Models;
using OnlineClassifiedsPlatform.AzureServiceBus.ServiceBusTopic;
using OnlineClassifiedsPlatform.Configuration;
using OnlineClassifiedsPlatform.SignalR.Interfaces;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.SignalR.Hubs
{
    [HubName("GoodsHub")]
    public class GoodsNotificationHub : Hub<IGoodsNotificationHub>
    {
        private readonly ServiceBusTopicConsumer _topicConsumer;
        private SubscriptionClient _subscription;
        private readonly string NotifyUserTopicPath = AzureServiceBusTopicConfiguration.CreateFromConfigurations().NotifyUserTopicPath;
        public IHubContext<GoodsNotificationHub, IGoodsNotificationHub> _strongChatHubContext { get; }

        private readonly string _connectionString = OnlineClassifiedsPlatformConfiguration.CreateFromConfigurations().AzureServiceBusConnectionString;

        public GoodsNotificationHub(IHubContext<GoodsNotificationHub, IGoodsNotificationHub> chatHubContext)
        {
            _strongChatHubContext = chatHubContext ?? throw new ArgumentNullException(nameof(_strongChatHubContext));
            _topicConsumer = new ServiceBusTopicConsumer(_connectionString);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendNoticeEventToClient($"{Context.ConnectionId} connected");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendNoticeEventToClient($"{Context.ConnectionId} connected");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var good = JsonConvert.DeserializeObject<GoodsModel>(Encoding.UTF8.GetString(message.Body));

            if (!string.IsNullOrEmpty(good.Name) && !string.IsNullOrEmpty(good.PaymentStatusName))
            {
                await _strongChatHubContext.Clients.All.SendNoticeEventToClient("Goods name: " + good.Name + 
                    ". Payment status: " + good.PaymentStatusName);
            }
            await _subscription.CompleteAsync(message.SystemProperties.LockToken);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            var exeptionMessage = arg.Exception.Message;
            return OnConnectedAsync();
        }

        public async void Start()
        {
            _subscription = await _topicConsumer.GetSubscriptionClient(NotifyUserTopicPath);
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            _subscription.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }
    }
}
