using System.Threading.Tasks;
using VideoConverter.Api.Models;

namespace azure_converter_project.SignalR
{
    public interface ITracksDisplay
    {
        Task RecieveTrack(Track track);
    }
}