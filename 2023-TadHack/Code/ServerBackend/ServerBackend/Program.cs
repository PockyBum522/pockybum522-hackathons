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

            await Task.Delay(10000);
        }
        
        // ReSharper disable once FunctionNeverReturns because it's not supposed to
    }

    private static async Task WriteImagesJsonIndex()
    {
        var imagesIndex = Directory.GetFiles(PathToStableDiffusionImages);

        var jsonString = "{\"";

        for (var i = 0; i < imagesIndex.Length; i++)
        {
            var fullPath = imagesIndex[i];

            jsonString += Path.GetFileName(fullPath);

            if (i < imagesIndex.Length - 1)
                jsonString += "\", \"";
        }

        jsonString += "\"}";

        Console.WriteLine("Writing:");
        Console.WriteLine(jsonString);
        
        var fullPathToJsonIndex = Path.Join(PathToStableDiffusionImages, "images_index.json");
        
        Console.WriteLine("To file:");
        Console.WriteLine(fullPathToJsonIndex);
        
        await File.WriteAllTextAsync(fullPathToJsonIndex, jsonString);
    }
}