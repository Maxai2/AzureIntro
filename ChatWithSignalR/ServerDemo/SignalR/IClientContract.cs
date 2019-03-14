using System.Threading.Tasks;
using SignalrServerDemo.Models;

namespace SignalrServerDemo.SignalR {
    public interface IClientContract {
        Task ReceiveMessage (MessageDto message);
        Task UserConnected (string user);
    }
}