using System.Threading.Tasks;

namespace Order.Services.Hub
{
    public interface IAppHub
    {
        Task SendNotification(string message);
    }
}