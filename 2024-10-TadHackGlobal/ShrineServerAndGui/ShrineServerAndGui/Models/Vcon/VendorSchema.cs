using Newtonsoft.Json;

namespace ShrineServerAndGui.Models.Vcon;

public class VendorSchema
{
    [JsonProperty("model")]
    public string Model { get; set; }

    [JsonProperty("prompt")]
    public string Prompt { get; set; }
}