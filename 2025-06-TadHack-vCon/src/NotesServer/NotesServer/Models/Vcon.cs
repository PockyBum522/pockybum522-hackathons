using Newtonsoft.Json;

namespace NotesServer.Models;

public class VconRoot
{
    [JsonProperty("uuid")] 
    public Guid Uuid { get; set; } = Guid.NewGuid();

    [JsonProperty("vcon")] 
    public string Vcon { get; set; } = "0.0.1";

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonProperty("redacted")]
    public Dictionary<string, object> Redacted { get; set; } = new ();

    [JsonProperty("group")]
    public List<object> Group { get; set; } = [];

    [JsonProperty("parties")]
    public List<Party> Parties { get; set; } = [];

    [JsonProperty("dialog")]
    public List<Dialog> Dialog { get; set; } = [];

    [JsonProperty("attachments")]
    public List<Attachment> Attachments { get; set; } = [];

    [JsonProperty("analysis")]
    public List<object> Analysis { get; set; } = [];
}

public class Party
{
    [JsonProperty("name")]
    public string Name { get; set; } = "";

    [JsonProperty("role")]
    public string Role { get; set; } = "";
}

public class Dialog
{
    [JsonProperty("type")]
    public string Type { get; set; } = "";

    [JsonProperty("start")]
    public DateTime Start { get; set; }

    [JsonProperty("parties")] 
    public List<int> Parties { get; set; } = [];

    [JsonProperty("mimetype")]
    public string MimeType { get; set; } = "";

    [JsonProperty("body")]
    public string Body { get; set; } = "";
}

public class Attachment
{
    [JsonProperty("type")]
    public string Type { get; set; } = "";

    [JsonProperty("body")]
    public List<string> Body { get; set; } = [];

    [JsonProperty("encoding")]
    public string Encoding { get; set; } = "";
}