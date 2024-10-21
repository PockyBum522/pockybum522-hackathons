using OpenAI.Chat;
using ShrineServerAndGui;

namespace ShrineBackendServer;

public class OpenAiRequester
{
    private static ChatClient? _openAiClient;

    public OpenAiRequester()
    {
        _openAiClient ??= new(model: "gpt-4o", apiKey: SECRETS.OpenAiApiKey); 
    }
    
    public double AnalyzeSentiment(string textToAnalyze)
    {
        _openAiClient ??= new(model: "gpt-4o", apiKey: SECRETS.OpenAiApiKey);

        var sentimentRequestPreamble =
            "On a rating scale of 0 to 1, where 0 is extremely emotionally negative, 0.5 is neutral, and 1 is extremely positive, what would you rate the following text?\n\n";
        
        var overallPositiveSentimentRequest = 
            sentimentRequestPreamble +
            textToAnalyze + "\n\n" +
            "Please respond only with a decimal number between 0 and 1 with no other text in your response.";
                
        ChatCompletion response = _openAiClient.CompleteChat( overallPositiveSentimentRequest);
        
        return double.Parse(
            response.Content[0].Text.Trim()); 
    }
    
    public List<string> GetMostEmotionalWords(string textToAnalyze)
    {
        _openAiClient ??= new(model: "gpt-4o", apiKey: SECRETS.OpenAiApiKey);

        var requestPreamble =
            "In the following text, what are the three most emotionally charged words?\n\n";
        
        var overallRequest = 
            requestPreamble +
            textToAnalyze + "\n\n" +
            "Please respond only with the three most emotionally charged words from the text, separated by spaces, and with no other text in your response.";
                
        ChatCompletion response = _openAiClient.CompleteChat( overallRequest);
        
        return response.Content[0].Text
            .Trim()
            .Split(" ")
            .ToList();
    }
    
    public string CleanAndProofText(string textToAnalyze)
    {
        _openAiClient ??= new(model: "gpt-4o", apiKey: SECRETS.OpenAiApiKey);

        var requestPreamble =
            "Please proofread and clean up the following text, including removing errant spaces, fixing capitalization, and correcting words where it is extremely obvious what the word should have been. Please do not change the meaning or wording of the text except for where it is extremely obvious that a word is a mistake::\n\n";
        
        var overallRequest = 
            requestPreamble +
            textToAnalyze + "\n\n" +
            "Please respond only with the corrected version of the original text, and with no other text in your response.";
                
        ChatCompletion response = _openAiClient.CompleteChat( overallRequest);
        
        return response.Content[0].Text;
    }
}