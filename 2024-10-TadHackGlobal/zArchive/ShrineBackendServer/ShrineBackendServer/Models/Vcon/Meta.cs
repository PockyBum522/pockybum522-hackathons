using Newtonsoft.Json;

namespace ShrineBackendServer.Models.Vcon;

public class Meta
{
    [JsonProperty("direction")]
    public string Direction { get; set; }

    [JsonProperty("disposition")]
    public string Disposition { get; set; }

    [JsonProperty("role")]
    public string Role { get; set; }
}