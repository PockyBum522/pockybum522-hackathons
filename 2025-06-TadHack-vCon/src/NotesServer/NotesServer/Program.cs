using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NotesServer.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace NotesServer;

class Program
{
    static async Task Main(string[] args)
    {
        // var jsonResponse = await QueryOpenAI();
        var jsonResponse = ExampleData.ExampleOpenAIJsonResponse;

        var nativeResponse = JsonConvert.DeserializeObject<OpenAIResponse>(jsonResponse);
        
        Console.WriteLine(nativeResponse.Choices.FirstOrDefault().Message.Content); // David is a bad influence
    }
    
    static async Task<string> QueryOpenAI()
    {
        var imagePath = "/home/jurrd3/repos/pockybum522-hackathons/2025-06-TadHack-vCon/example-input/model-numbers-easier/PXL_20250516_132015872.jpg"; // Local image path

        byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);
        string base64Image = Convert.ToBase64String(imageBytes);

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

        string jsonPayload = JsonSerializer.Serialize(payload);

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SECRETS.OpenAIAPIKey);

        var response = await client.PostAsync(
            "https://api.openai.com/v1/chat/completions",
            new StringContent(jsonPayload, Encoding.UTF8, "application/json")
        );

        string result = await response.Content.ReadAsStringAsync();
        // Console.WriteLine("Response:\n" + result);
        return result;
    }
}