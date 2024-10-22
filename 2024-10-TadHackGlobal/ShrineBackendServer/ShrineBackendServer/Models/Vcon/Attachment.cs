using Newtonsoft.Json;

namespace ShrineBackendServer.Models.Vcon;

public class Attachment
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("encoding")]
    public string Encoding { get; set; }

    [JsonProperty("body")]
    public Body Body { get; set; }
}