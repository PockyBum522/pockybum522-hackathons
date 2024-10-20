using Newtonsoft.Json;

namespace ShrineServerAndGui.Models.Vcon;

public class Party
{
    [JsonProperty("tel")]
    public string Tel { get; set; }

    [JsonProperty("meta")]
    public Meta Meta { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("mailto")]
    public string Mailto { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }
}