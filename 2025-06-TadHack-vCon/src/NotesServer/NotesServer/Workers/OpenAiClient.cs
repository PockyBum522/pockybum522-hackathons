using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using JetBrains.Annotations;
using Serilog;

namespace NotesServer;

public class OpenAiClient
{
    private readonly ILogger _logger;

    public OpenAiClient(ILogger logger)
    {
        _logger = logger;
    }
    
    [PublicAPI]
    public async Task<string> QueryOpenAi(string imagePath)
    {
        var imageBytes = await File.ReadAllBytesAsync(imagePath);
        var base64Image = Convert.ToBase64String(imageBytes);

        var payload = new
        {
            model = "gpt-4o",
            messages = new[]
            {
                new {
                    role = "user",
                    content = new object[]
                    {
                        new { type = "text", text = "This is a documentation image taken for someone's notes. Please transcribe the most prominent text in this image. Format your response as only the transcribed text with no other words in your response." },
                        new {
                            type = "image_url",
                            image_url = new {
                                url = $"data:image/jpeg;base64,{base64Image}",
                                detail = "auto"
                            }
                        }
                    }
                }
            }
        };

        var jsonPayload = JsonSerializer.Serialize(payload);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SECRETS.OpenAiApiKey);

        var response = await client.PostAsync(
            "https://api.openai.com/v1/chat/completions",
            new StringContent(jsonPayload, Encoding.UTF8, "application/json")
        );

        var result = await response.Content.ReadAsStringAsync();
 
        _logger.Debug("OpenAI Response: {Result}" , result);
        
        return result;
    }
}