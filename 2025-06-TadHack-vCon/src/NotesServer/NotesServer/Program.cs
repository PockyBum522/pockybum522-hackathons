using System.Net.Http.Headers;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json;
using NotesServer.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace NotesServer;

static class Program
{
    internal static void Main(string[] args)
    {
        // var imagePath = "/home/jurrd3/repos/pockybum522-hackathons/2025-06-TadHack-vCon/example-input/model-numbers-easier/PXL_20250516_132015872.jpg"; // Local image path
        
        // Fake OpenAI Query
        // var jsonResponse = await QueryOpenAi(imagePath);
        // var jsonResponse = ExampleData.ExampleOpenAiJsonResponse;
        // var nativeResponse = JsonConvert.DeserializeObject<OpenAiResponse>(jsonResponse);
        // Console.WriteLine(nativeResponse.Choices.FirstOrDefault().Message.Content); // David is a bad influence
        
        // EXIF Data extraction from image (like extracting vanilla)
        // var exifData = ExifHelper.ExtractFromImage(imagePath);
        // Console.WriteLine($"lat: { exifData.GpsLatitude }, long: { exifData.GpsLongitude }, date: { exifData.TakenAt }");
        
        // var nativeVcon = JsonConvert.DeserializeObject<VconRoot>(ExampleData.ExampleVcon);
        //
        // Console.WriteLine(nativeVcon.Parties);

        var testVcon = new VconRoot
        {
            Vcon = "0.0.1",
            CreatedAt = DateTime.Now
        };


        testVcon.Parties.Add(
            new Party()
            {
                Name = "OpenAI",
                Role = "LLM"
            }
        );
        
        testVcon.Dialog.Add(
            new Dialog()
            {
                Body = "P/NO. SN20P34496     FRU NO. 01YP680\\nLCFC P/N  PK131671B00\\nSG-90850-XUA  01  NUM-BL US",
                MimeType = "text/plain",
                Parties = [0],
                Start = DateTime.Now,
                Type = "text"
            }
        );
        
        testVcon.Attachments.Add(
            new Attachment()
            {
                Type = "tags",
                Body = [
                    "location:28.644655, -81.465546"
                ],
                Encoding = "json"
            }
        );
        
        var testVconJson = JsonConvert.SerializeObject(testVcon);
        Console.WriteLine(testVconJson);
    }
    
    [PublicAPI]
    static async Task<string> QueryOpenAi(string imagePath)
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
        // Console.WriteLine("Response:\n" + result);
        return result;
    }
}