using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using VideoConverter.Api.Models;

namespace azure_converter_project.SignalR
{
    public class CustomHub : Hub<ITracksDisplay>
    {
        static private Dictionary<string, string> connections = new Dictionary<string, string>();

        public override Task OnConnectedAsync()
        {
            connections.Add(this.Context.UserIdentifier, this.Context.ConnectionId);
            return Task.CompletedTask;
        }

        // public void SendTrack(Track track, string id)
        // {
        //     this.Clients.Client(connections[id]).RecieveTrack(track);
        // }
    }
}