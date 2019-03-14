using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalrServerDemo.Models;

namespace SignalrServerDemo.SignalR {
    [Authorize (Roles = "User")]
    public class MyHub : Hub<IClientContract> {
        public void SendMessage (MessageDto message, string room) {
            // this.Clients.All.ReceiveMessage (message);
            this.Clients.Group(room).ReceiveMessage (message);
        }

        private void UserConnected(string user, string room) {
            // this.Clients.All.UserConnected(user);
            this.Clients.Group(room).UserConnected(user);
        }

        private void SendDirect(string userId, MessageDto message) {
            this.Clients.User(userId).ReceiveMessage(message);
            // this.Clients.Client(user).ReceiveMessage(message);
        }

        public override async Task OnConnectedAsync () {
            var req = this.Context.GetHttpContext().Request;
            var room = req.Query["room"].FirstOrDefault() ?? "general";
            // if (this.Context.User.Identity.Name.ToUpper().Equals("qwe123")) {
            //     await this.Groups.AddToGroupAsync(this.Context.ConnectionId, "Designer");
            // }
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, room);
            this.UserConnected(this.Context.User.Identity.Name, room);
        }
    }
}