using System.Net.Http.Headers;

namespace DezgoStableDiffusionTest;

class Program
{
    public static async Task Main()
    {
        var llmUserPrompt = "well by golly gee whiz it was fuckin evanescence, i have listened to them my whole life ever since my middle school graduation";

        var llmPromptPrefix = "A creative interpretation of the following, in a random art style: ";
        
        var client = new HttpClient();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("https://api.dezgo.com/text2image"),
            Headers =
            {
                { "X-Dezgo-Key", SECRETS.DezgoApiKey },
                // { "Content-Type", "application/json" },
            },
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "width", "1024" },
                { "height", "768" },
                { "model", "epic_diffusion_1_1" },
                { "negative_prompt", "ugly, tiling, poorly drawn hands, poorly drawn feet, poorly drawn face, out of frame, extra limbs, disfigured, deformed, body out of frame, blurry, bad anatomy, blurred, watermark, grainy, signature, cut off, draft" },
                { "sampler", "dpmpp_2m_karras" },
                { "steps", "30" },
                { "guidance", "10" },
                { "prompt", llmPromptPrefix + llmUserPrompt },
                { "upscale", "2" },
                
            }),
        };

        using var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var responseHeaders = response.Headers;

        foreach (var rHeader in responseHeaders)
        {
            Console.WriteLine($"Header key: {rHeader.Key}");

            var valueCounter = 1;
            foreach (var hValue in rHeader.Value)
            {
                Console.WriteLine($"Header value {valueCounter++}: {hValue}");
            }
        }

        Console.WriteLine();


        var body = await response.Content.ReadAsStreamAsync();

        var filesafeTimeStamp = DateTimeOffset.Now.ToString("T").Replace(":", "-");
        
        var outFilePath = @$"D:\Dropbox\Documents\Desktop\test_{filesafeTimeStamp}.png"; 
        
        File.Delete(outFilePath);
        
        SaveStreamAsFile(outFilePath, body);
    }

    private static void SaveStreamAsFile(string fullFilePath, Stream inputStream) 
    {  
        using var outputFileStream = new FileStream(fullFilePath, FileMode.Create);
        
        inputStream.CopyTo(outputFileStream);
    } 
}