using System.Web;
using DezgoStableDiffusionTest;
using Newtonsoft.Json.Linq;

namespace ServerBackend;

public static class Program
{
    private const string KeysValuesCachePath = @"C:\Users\Public\Documents\cached_key_value_pairs.txt"; 
    private const string PathToStableDiffusionImages = @"C:\Users\jurrd\source\repos\pockybum522-hackathons\2023-TadHack\Code\Demos\TypescriptProjectDemo\resources\images";
    
    /// <summary>
    /// Worker loop that does all backend tasks
    /// </summary>
    public static async Task Main()
    {
        while (true)
        {
            await WriteImagesJsonIndex();

            var stacuityKeysValuesList = await MakeStacuityKeysValuesList();
            
            foreach (var rawValue in stacuityKeysValuesList)
            {
                var keyValueStringSeenAlready = KeyValuePairSeenAlready(rawValue);
                
                if (keyValueStringSeenAlready) continue;

                await SaveNewDezgoGeneratedImage(
                    ParseStacuityKey(rawValue),
                    ParseStacuityValue(rawValue));

                CacheRawKeyValueStringAsSeen(rawValue);
            }
            
            await Task.Delay(10000);
        }
        
        // ReSharper disable once FunctionNeverReturns because it's not supposed to
    }

    private static async Task SaveNewDezgoGeneratedImage(string promptUserSaw, string imagePrompt)
    {
        Console.WriteLine($"About to generate image on Dezgo using prompt: {imagePrompt}");
        
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
                { "prompt", llmPromptPrefix + imagePrompt },
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

        
        var filesafeTimeStamp = DateTimeOffset.Now.ToString("d").Replace("/", "-");
        filesafeTimeStamp += '_';
        filesafeTimeStamp += DateTimeOffset.Now.ToString("T").Replace(":", "-");

        var filesafeUserPrompt = promptUserSaw.Replace("?", "");
        filesafeUserPrompt = filesafeUserPrompt.Replace(".", "");
        filesafeUserPrompt = filesafeUserPrompt.Replace(" ", "_");
        filesafeUserPrompt = filesafeUserPrompt.ToLower();
        
        var outFileName = $"{filesafeUserPrompt}_{filesafeTimeStamp}.png";

        var fullPathToSaveTo = Path.Join(PathToStableDiffusionImages, outFileName);
        
        SaveStreamAsFile(fullPathToSaveTo, body);
    }

    private static void SaveStreamAsFile(string fullFilePath, Stream inputStream) 
    {  
        using var outputFileStream = new FileStream(fullFilePath, FileMode.Create);
        
        inputStream.CopyTo(outputFileStream);
    } 
    
    private static void CacheRawKeyValueStringAsSeen(string rawValue)
    {
        if (!File.Exists(KeysValuesCachePath))
            File.Create(KeysValuesCachePath).Close();

        File.AppendAllText(KeysValuesCachePath, Environment.NewLine + rawValue);
    }

    private static bool KeyValuePairSeenAlready(string rawValueToCheck)
    {
        if (!File.Exists(KeysValuesCachePath))
            File.Create(KeysValuesCachePath).Close();
        
        var cachedKeysValues = File.ReadAllLines(KeysValuesCachePath);

        var isKeyValueInCache = false;
        
        foreach (var line in cachedKeysValues)
        {
            if (line == rawValueToCheck)
                isKeyValueInCache = true;
        }

        return isKeyValueInCache;
    }

    private static string ParseStacuityKey(string rawValue)
    {
        var splitKey = rawValue.Split("---");
                
        return splitKey[0];
    }

    private static string ParseStacuityValue(string rawValue)
    {
        var splitKey = rawValue.Split("---");
                
        return splitKey[1];
    }

    private static async Task<List<string>> MakeStacuityKeysValuesList()
    {
        var keyValueJson = await CheckForNewStacuityKeys();
            
        var decodedKeyValueJson = HttpUtility.UrlDecode(keyValueJson);

        dynamic stacuityKeysValuesObject = JObject.Parse(decodedKeyValueJson);

        var stacuityKeysValuesList = new List<string>();
            
        foreach (var key in stacuityKeysValuesObject.data)
        {
            var rawValue = (string)key.value;

            stacuityKeysValuesList.Add(rawValue);
        }

        return stacuityKeysValuesList;
    }

    private static async Task WriteImagesJsonIndex()
    {
        if (!Directory.Exists(PathToStableDiffusionImages))
        {
            Console.WriteLine($"Could not find anything at path: {PathToStableDiffusionImages}");
            return;
        }
        
        var imagesIndex = Directory.GetFiles(PathToStableDiffusionImages);

        var jsonString = "{ \"images_filenames\" : [ \"";

        for (var i = 0; i < imagesIndex.Length; i++)
        {
            var fullPath = imagesIndex[i];

            jsonString += Path.GetFileName(fullPath);

            if (i < imagesIndex.Length - 1)
                jsonString += "\", \"";
        }

        jsonString += "\" ] }";

        Console.WriteLine("Writing:");
        Console.WriteLine(jsonString);
        
        var fullPathToJsonIndex = Path.Join(PathToStableDiffusionImages, "images_index.json");
        
        Console.WriteLine("To file:");
        Console.WriteLine(fullPathToJsonIndex);
        
        await File.WriteAllTextAsync(fullPathToJsonIndex, jsonString);
    }

    private static async Task<string> CheckForNewStacuityKeys()
    {
        var client = new HttpClient();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.stacuity.com/api/v1/endpoints/b257522a-4d83-4f3e-8954-71d296bbf7c1/keyvalues"),
            Headers =
            {
                { "Authorization", $"Bearer {SECRETS.StacuityApiBearerToken}" },
            },
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

        var body = await response.Content.ReadAsStringAsync();

        return body;
    }    
}