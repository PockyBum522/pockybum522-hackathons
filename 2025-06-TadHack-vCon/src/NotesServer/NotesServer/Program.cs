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
        var imagePath = GetImagePathPerMachine(Environment.UserName);
        
        // Fake OpenAI Query
        // var jsonResponse = await QueryOpenAi(imagePath);
        // var jsonResponse = ExampleData.ExampleOpenAiJsonResponse;
        // var nativeResponse = JsonConvert.DeserializeObject<OpenAiResponse>(jsonResponse);
        // Console.WriteLine(nativeResponse.Choices.FirstOrDefault().Message.Content); // David is a bad influence
        
        // var nativeVcon = JsonConvert.DeserializeObject<VconRoot>(ExampleData.ExampleVcon);
        //
        // Console.WriteLine(nativeVcon.Parties);
        
        // EXIF Data extraction from image (like extracting vanilla)
        var exifData = ExifDataExtractor.FromImage(imagePath);
        Console.WriteLine($"lat: { exifData.GpsLatitude }, long: { exifData.GpsLongitude }, date: { exifData.TakenAt }");
        
        var testVcon = GenerateInitialVconForNote(
            "P/NO. SN20P34496     FRU NO. 01YP680\\nLCFC P/N  PK131671B00\\nSG-90850-XUA  01  NUM-BL US", exifData);
        
        var testVconJson = JsonConvert.SerializeObject(testVcon);
        Console.WriteLine(testVconJson);
    }
    
    [PublicAPI]
    private static async Task<string> QueryOpenAi(string imagePath)
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

    private static VconRoot GenerateInitialVconForNote(string vconBody, ImageExifData exifData)
    {
        var generatedVcon = new VconRoot();

        generatedVcon.Parties.Add(
            new Party()
            {
                Name = "OpenAi",
                Role = "Transcriber"
            }
        );
        
        generatedVcon.Dialog.Add(
            new Dialog()
            {
                Body = vconBody,
                MimeType = "text/plain",
                Parties = [0],
                Start = exifData.TakenAt,
                Type = "text"
            }
        );
        
        generatedVcon.Attachments.Add(
            new Attachment()
            {
                Type = "tags",
                Body = [$"location:{exifData.GpsLatitude}, -{exifData.GpsLongitude}"],
                Encoding = "json"
            }
        );

        return generatedVcon;
    }
    
    private static string GetImagePathPerMachine(string userName)
    {
        // Local image path on Jurrd's machine
        var jaredReposPath = "/home/jurrd3/repos";

        var davidReposPath = "/media/secondary/repos";
        
        var imagePathInRepo =
            "pockybum522-hackathons/2025-06-TadHack-vCon/example-input/model-numbers-easier/PXL_20250516_132015872.jpg";
        
        if (userName.Contains("david", StringComparison.InvariantCultureIgnoreCase))
            return Path.Join(davidReposPath, imagePathInRepo);

        if (userName.Contains("jurrd", StringComparison.InvariantCultureIgnoreCase))
            return Path.Join(jaredReposPath, imagePathInRepo);
        
        return "";
    }
}