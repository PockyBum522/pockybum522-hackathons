using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShrineServerAndGui.Models.Vcon;

public class Analysis
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("dialog")]
    public int Dialog { get; set; }

    [JsonProperty("vendor")]
    public string Vendor { get; set; }

    [JsonProperty("encoding")]
    public string Encoding { get; set; }

    [JsonProperty("body")] 
    public List<Body> Body { get; set; } = [];

    [JsonProperty("vendor_schema")]
    public VendorSchema VendorSchema { get; set; }
}