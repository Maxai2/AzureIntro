namespace VideoConverter.Api.Models {
    public class Track {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string VideoUrl { get; set; }
        public string AudioUrl { get; set; }
    }
}