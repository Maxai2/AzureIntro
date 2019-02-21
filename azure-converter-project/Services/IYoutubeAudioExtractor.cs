using System.Threading.Tasks;
using VideoConverter.Api.Models;

namespace VideoConverter.Api.Services {
    public interface IYoutubeAudioExtractor {
        Task<ConvertedItem> Extract (string videoUrl);
    }
}