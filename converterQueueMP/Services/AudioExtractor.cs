using System.IO;
using System.Threading.Tasks;
using ConverterApp.Interfaces;
using ConverterApp.Models;
using MediaToolkit;
using MediaToolkit.Model;
using VideoLibrary;

namespace ConverterApp.Services {
    public class AudioExtractor : IAudioExtractor {
        public async Task<ConvertedItem> Extract (string code) {
            var vid = await YouTube.Default.GetVideoAsync ("https://youtube.com/watch?v=" + code);

            var tempFilePath = Path.GetRandomFileName ();
            File.WriteAllBytes (tempFilePath, await vid.GetBytesAsync ());

            var inputFile = new MediaFile { Filename = tempFilePath };
            var outputFile = new MediaFile { Filename = $"{tempFilePath}.mp3" };
            using (var engine = new Engine ("./ffmpeg.exe")) {
                engine.GetMetadata (inputFile);
                engine.Convert (inputFile, outputFile);
            }

            var stream = new MemoryStream (File.ReadAllBytes (outputFile.Filename));

            File.Delete (inputFile.Filename);
            File.Delete (outputFile.Filename);

            return new ConvertedItem (stream, vid.Title, vid.GetUri (), code);
        }
    }
}