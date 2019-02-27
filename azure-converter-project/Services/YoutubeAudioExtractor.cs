using System;
using System.IO;
using System.Threading.Tasks;
using MediaToolkit;
using MediaToolkit.Model;
using VideoConverter.Api.Models;
using VideoLibrary;
namespace VideoConverter.Api.Services {
    public class YoutubeAudioExtractor : IYoutubeAudioExtractor {
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

            return new ConvertedItem (stream, vid.Title, vid.GetUri(), code);
        }
    }
}