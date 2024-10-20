using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShrineServerAndGui.Models.Vcon;

public class Dialog
{
    [JsonProperty("alg")]
    public string Alg { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("meta")]
    public Meta Meta { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("start")]
    public DateTime Start { get; set; }

    [JsonProperty("parties")]
    public List<int> Parties { get; set; }

    [JsonProperty("duration")]
    public double Duration { get; set; }

    [JsonProperty("filename")]
    public string Filename { get; set; }

    [JsonProperty("mimetype")]
    public string Mimetype { get; set; }

    [JsonProperty("signature")]
    public string Signature { get; set; }
}