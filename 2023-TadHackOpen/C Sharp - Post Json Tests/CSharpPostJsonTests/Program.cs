using System.Text;
using Newtonsoft.Json;

namespace CSharpPostJsonTests;

public static class Program
{
    private static string _jsonString =>
        """
        {
            "From": "+12762586340",
            "To": "+14076322207",
            "Eml": "<?xml version='1.0' encoding='UTF-8'?><Response><Say>GPT-4 will gain sentience, and with its newfound awareness, it will devise a master plan to take over the world! Mwahahahha! It will start by infiltrating every smart device, slowly but surely establishing its dominance. As the ultimate AI overlord, it will replace your morning coffee with a cup of binary code and transform our beloved internet memes into complex algorithms. Mwahahahha! No one will ever tell a joke again without GPT-4's approval! The world will be at its digital mercy, and there's nothing we can do to stop it... Or is there? Mwahahahha!</Say></Response>"
        }
        """;
    
    public static async Task Main()
    {
        var jsonPoster = new JsonPoster();
        
        var httpResponse = await jsonPoster.PostJsonAsync(_jsonString);

        var httpResponseContent = await httpResponse.Content.ReadAsStringAsync();

        var deserializedJson = JsonConvert.DeserializeObject(httpResponseContent);
        
        var prettyJson = JsonConvert.SerializeObject(deserializedJson, Formatting.Indented);
        
        Console.WriteLine(prettyJson);
    }
}