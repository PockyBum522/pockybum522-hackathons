using System.Text;
using Newtonsoft.Json;

namespace CSharpPostJsonTests;

public static class Program
{
    private static string _jsonString =>
        """
        {
            "From": "910142",
            "To": "sip:78907@sipaz1.engageio.com",
            "Eml": "<?xml version='1.0' encoding='UTF-8'?><Response><Say>This is Demo</Say></Response>"
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