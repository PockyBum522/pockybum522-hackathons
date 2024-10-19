using Newtonsoft.Json;

namespace ShrineBackendServer.Models;

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

public class Attachment
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("encoding")]
    public string Encoding { get; set; }

    [JsonProperty("body")]
    public Body Body { get; set; }
}

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

public class Meta
{
    [JsonProperty("direction")]
    public string Direction { get; set; }

    [JsonProperty("disposition")]
    public string Disposition { get; set; }

    [JsonProperty("role")]
    public string Role { get; set; }
}

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


public class VendorSchema
{
    [JsonProperty("model")]
    public string Model { get; set; }

    [JsonProperty("prompt")]
    public string Prompt { get; set; }
}