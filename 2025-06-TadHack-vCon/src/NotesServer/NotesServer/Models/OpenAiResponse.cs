using Newtonsoft.Json;

namespace NotesServer.Models;

public class OpenAiResponse
{
    [JsonProperty("id")] 
    public string Id { get; set; } = "";

    [JsonProperty("object")]
    public string Object { get; set; } = "";

    [JsonProperty("created")]
    public long Created { get; set; }

    [JsonProperty("model")]
    public string Model { get; set; } = "";

    [JsonProperty("choices")] 
    public List<Choice> Choices { get; set; } = new();

    [JsonProperty("usage")]
    public Usage Usage { get; set; } = new();
}

public class Choice
{
    [JsonProperty("index")]
    public int Index { get; set; }

    [JsonProperty("message")]
    public Message Message { get; set; } = new();

    [JsonProperty("finish_reason")]
    public string FinishReason { get; set; } = "";
}

public class Message
{
    [JsonProperty("role")]
    public string Role { get; set; } = "";

    [JsonProperty("content")]
    public string Content { get; set; } = "";
}

public class Usage
{
    [JsonProperty("prompt_tokens")]
    public int PromptTokens { get; set; }

    [JsonProperty("completion_tokens")]
    public int CompletionTokens { get; set; }

    [JsonProperty("total_tokens")]
    public int TotalTokens { get; set; }
}