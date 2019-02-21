using System.IO;
using System.Threading.Tasks;

namespace VideoConverter.Api.Services {
    public interface IMediaStorageService {
        Task<string> Upload (Stream stream, string name = null, string extension = null);
    }
}