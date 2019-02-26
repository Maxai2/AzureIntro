using Newtonsoft.Json;

namespace VideoConverter.Api.Models {
    public abstract class ModelBase {
        [JsonProperty ("id")]
        public string Id { get; set; }
    }
}