using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShrineServerAndGui.Models.Vcon;

public class Vcon
{
    [JsonProperty("uuid")] 
    public string Uuid { get; set; } = Guid.NewGuid().ToString();

    [JsonProperty("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    [JsonProperty("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

    [JsonProperty("dialog")]
    public List<Dialog> Dialog { get; set; } = [];

    [JsonProperty("parties")] 
    public List<Party> Parties { get; set; } = [];

    [JsonProperty("attachments")] 
    public List<Attachment> Attachments { get; set; } = [];

    [JsonProperty("analysis")]
    public List<Analysis> Analysis { get; set; } = [];
}