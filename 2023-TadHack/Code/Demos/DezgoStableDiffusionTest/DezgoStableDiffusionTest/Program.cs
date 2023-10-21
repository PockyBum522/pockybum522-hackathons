using System.Net.Http.Headers;

namespace DezgoStableDiffusionTest;

class Program
{
    public static async Task Main()
    {
        // User response examples:
        
        // What was your favorite song or artist when you were 17 years old and why?
        // const string llmUserPrompt = "well by golly gee whiz it was fuckin evanescence, i have listened to them my whole life ever since my middle school graduation";

        // What is your favorite comfort food and why?
        const string llmUserPrompt = "I love macaroni and cheese. It is so rich and delicious and I think it is my favorite thing that is yellow";
        
        // What is your favorite beverage and why?
        // const string llmUserPrompt = "I love coffee! Coffee is so delicious. It is my favorite thing to drink on cold winter days";

        // Describe your favorite hobby
        // const string llmUserPrompt = "I like to sharpen knives and then they are fun to use to cook with when they are nice and sharp";

        // Describe your favorite place on earth
        // const string llmUserPrompt = "My favorite place on earth is North Carolina, especially when the leaves are turning pretty colors in fall. I love anywhere that has lots of trees and rivers";

        // Describe your dream vacation
        // const string llmUserPrompt = "My dream vacation would be to Japan because I want to see all the hentai";

        // Describe your favorite animal
        // const string llmUserPrompt = "My favorite animal is the gray wolf, I like that they live in packs and help each other out";

        // Describe your favorite book
        // const string llmUserPrompt = "My favorite book is the Bible because Jesus was a cool dude who kills aliens and doesn't afraid of anything";

        // Describe your favorite halloween costume
        // const string llmUserPrompt = "The best Halloween costume is a Jedi night, especially if you have a lightsaber and everyone knows the best color. Lightsaber is purple because Samuel l. Jackson actually requested that his lightsaber be purple";

        // What is your favorite color and why
        // const string llmUserPrompt = "My favorite color is purple because that is the color of mace windows lightsaber and he is the coolest Jedi";

        // What was your favorite toy as a child
        // const string llmUserPrompt = "My favorite toy is a child was that carpet that has like a town on it and it has cars and streets and roads and rivers and other things and I think there's people on it and then there's probably some other things and this is kind of just rambling and I just want to see what happens";

        // Prefix for the LLM, will be prepended to the prompt
        const string llmPromptPrefix = "A creative interpretation of the following, in a random art style: ";
        
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