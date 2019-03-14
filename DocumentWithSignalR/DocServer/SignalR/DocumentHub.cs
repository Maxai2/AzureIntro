using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace DocServer.SignalR {
    public interface IDocuClient {
        Task CreateDocu(string name);
        Task EditDocu(string name, string text);
    }

    public class DocumentHub : Hub<IDocuClient> {
        // static private Dictionary<> 
    }
}