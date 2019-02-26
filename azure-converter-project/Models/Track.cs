using Newtonsoft.Json;

namespace VideoConverter.Api.Models {
    public class Track : ModelBase {
        [JsonProperty ("title")]
        public string Title { get; set; }

        [JsonProperty ("artist")]
        public string Artist { get; set; }

        [JsonProperty ("album")]
        public string Album { get; set; }

        [JsonProperty ("videoUrl")]
        public string VideoUrl { get; set; }

        [JsonProperty ("audioUrl")]
        public string AudioUrl { get; set; }
    }
}