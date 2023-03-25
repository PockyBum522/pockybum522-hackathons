using Newtonsoft.Json;

namespace CSharpPostJsonTests;

public static class Program
{
    private static string _emlString => """<?xml version="1.0" encoding="UTF-8"?><Response><Say>This is Demo</Say></Response>""";
    
    private static string _jsonString =>
        $$"""
        {
            "From": "910412",
            "To": "+14076322207",
            "Eml": {{JsonConvert.ToString(_emlString)}}
        }
        """;
    
    public static async Task Main()
    {
        Console.WriteLine(_jsonString);
        
        var jsonPoster = new JsonPoster();
        
        var response = await jsonPoster.PostJsonAsync(_jsonString, "https://apigateway.engagedigital.ai/api/v1/accounts/AC-e815a968-966e-4387-a175-b62ebd6ce36a/call");

        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }
    
    // """
    //         {
    //             "From": "+0910412",
    //             "To": "+14076322207",
    //             "ApplicationID": "VDT-ID",
    //             "StatusCallback": "http://www.example.com/event",
    //             "StatusCallbackMethod": "POST",
    //             "StatusCallbackEvent": "initiated, ringing, answered, completed",
    //             "Type": "voice",
    //             "Bridge": "none"
    //         }
    //         """;
    
    
}