using System.IO;

namespace VideoConverter.Api.Models {
    public class ConvertedItem {
        public ConvertedItem (Stream stream, string title, string videoUrl, string code) {
            this.Stream = stream;
            this.Title = title;
            this.VideoUrl = videoUrl;
            this.Code = code;
        }
        public Stream Stream { get; }
        public string Title { get; }
        public string VideoUrl { get; }
        public string Code { get; }
    }
}