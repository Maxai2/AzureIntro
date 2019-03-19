using System.Threading.Tasks;
using ConverterApp.Models;

namespace ConverterApp.Interfaces {
    public interface IAudioExtractor {
        Task<ConvertedItem> Extract (string code);
    }
}