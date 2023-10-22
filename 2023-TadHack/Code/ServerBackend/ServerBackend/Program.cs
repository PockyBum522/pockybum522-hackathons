using System.Web;
using DezgoStableDiffusionTest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServerBackend;

public static class Program
{
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
                Console.Write("Key: ");
                Console.WriteLine(
                    ParseStacuityKey(rawValue));
                
                Console.Write("Value: ");
                Console.WriteLine(
                    ParseStacuityValue(rawValue));
            }
            
            await Task.Delay(10000);
        }
        
        // ReSharper disable once FunctionNeverReturns because it's not supposed to
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