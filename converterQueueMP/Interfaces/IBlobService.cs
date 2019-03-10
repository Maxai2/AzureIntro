using System.IO;
using System.Threading.Tasks;

namespace ConverterApp.Interfaces {
    public interface IBlobService {
        Task<string> Upload (Stream stream, string name, string extension);
    }
}