using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ServerDemo.SignalR {
    // public interface IChatClientContact {
    //     Task ReceiveMessage(string message);
    // }

    // public class ChatHub : Hub<IChatClientContact> {
    //     static private Dictionary<string, string> connections = new Dictionary<string, string>();

    //     public override Task OnConnectedAsync() {
    //         var httpContext = this.Context.GetHttpContext();
    //         var name = httpContext.Request.Query["name"].FirstOrDefault();
    //         connections.Add(name, this.Context.ConnectionId);
    //         // Console.WriteLine($"{Context.UserIdentifier} connected");
    //         return Task.CompletedTask;
    //     }

    //     public override Task OnDisconnectedAsync(Exception ex) {
    //         var httpContext = this.Context.GetHttpContext();
    //         var name = httpContext.Request.Query["name"].FirstOrDefault();
    //         connections.Add(name, this.Context.ConnectionId);
    //         // Console.WriteLine($"{Context.UserIdentifier} connected");
    //         return Task.CompletedTask;
    //     }

    //     public Task SendMessage(string message) {
    //         // return this.Clients.All.SendAsync("ReceiveMessage", message);
    //         // return this.Clients.All.ReceiveMessage(message);

    //         if (connections.TryGetValue("Qwerty", out var qwerty)) {
    //             return this.Clients.AllExcept(qwerty).ReceiveMessage(message);
    //         }

    //         return this.Clients.All.ReceiveMessage(message);
    //     }
    // }
}