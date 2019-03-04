using Newtonsoft.Json;

namespace Company.Models {
    public class CustomFile {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("blobName")]
        public string BlobName { get; set; }
        [JsonProperty("blobLength")]
        public long BlobLength { get; set; }
    }
}