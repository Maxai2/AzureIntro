using System.Threading.Tasks;
using ConverterApp.Models;
using Refit;

namespace ConverterApp.Interfaces {
    public interface ITracksApi {
        [Post("/api/tracks/info")]
        Task AddTrack(Track track);
    }
}