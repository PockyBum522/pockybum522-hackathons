using DezgoStableDiffusionTest;

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

            await CheckForNewStacuityKeys();

            await Task.Delay(10000);
        }
        
        // ReSharper disable once FunctionNeverReturns because it's not supposed to
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

    private static async Task CheckForNewStacuityKeys()
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

        Console.WriteLine("Response:");
        Console.WriteLine(body);
    }    
}