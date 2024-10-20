using System;
using Newtonsoft.Json;

namespace ShrineServerAndGui.Models.Vcon;

public class Body
{
    [JsonProperty("agent_name")]
    public string AgentName { get; set; }

    [JsonProperty("customer_name")]
    public string CustomerName { get; set; }

    [JsonProperty("business")]
    public string Business { get; set; }

    [JsonProperty("problem")]
    public string Problem { get; set; }

    [JsonProperty("emotion")]
    public double Emotion { get; set; }

    [JsonProperty("prompt")]
    public string Prompt { get; set; }

    [JsonProperty("created_on")]
    public DateTime CreatedOn { get; set; }

    [JsonProperty("model")]
    public string Model { get; set; }

    [JsonProperty("speaker")]
    public string Speaker { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }
}