using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.SignalR.Interfaces
{
    public interface IGoodsNotificationHub
    {
        Task SendNoticeEventToClient(string message);
    }
}
