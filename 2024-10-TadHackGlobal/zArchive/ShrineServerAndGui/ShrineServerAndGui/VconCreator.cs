using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShrineServerAndGui.Models.Vcon;

namespace ShrineServerAndGui;

public class VconCreator
{
    private static OpenAiRequester? _openAiRequester;
    
    public VconCreator()
    {
        _openAiRequester ??= new OpenAiRequester();
    }
    
    public async Task<string> GetVconJsonString(string fullSpokenWords)
    {
        _openAiRequester ??= new OpenAiRequester();
        
        var vcon = new Vcon();
        
        var vconDialog = new Dialog()
        {
            Alg = "SHA-512",
            Duration = 30.0,
            Parties = [0],
            Start = DateTime.Now - TimeSpan.FromSeconds(32)
        };

        vcon.Dialog.Add(vconDialog);
        
        var vconParty = new Party()
        {
            Name = "Parishioner"
        };
        
        vcon.Parties.Add(vconParty);
        
        var cleanedSpokenWords = _openAiRequester.CleanAndProofText(fullSpokenWords);

        await Task.Delay(100);
        
        var listOfMostEmotionallyChargedWords = 
                _openAiRequester.GetMostEmotionalWords(cleanedSpokenWords);

        await Task.Delay(100);
        
        var emotionallyChargedWords = new List<Attachment>();
        
        foreach (var emotionalWord in listOfMostEmotionallyChargedWords)
        {
            var emotionallyChargedWordsAttachment = new Attachment
            {
                Type = "MostEmotionallyChargedWord",
                
                Body = new Body()
                {
                    Message = emotionalWord
                }
            };

            emotionallyChargedWords.Add(emotionallyChargedWordsAttachment);
        }
        
        vcon.Attachments = emotionallyChargedWords;
        
        var fullSpokenWordsBody = new Body()
        {
            Message = fullSpokenWords
        };
        
        var vconAnalysis = new Analysis()
        {
            Type = "transcript",
            Dialog = 0,
            Vendor = "Temple of Computing",
            Encoding = "UTF-8"
        };
        
        vconAnalysis.Body.Add(
            new Body()
            {
                Speaker = "Parishioner",
                Message = cleanedSpokenWords,
                Emotion = _openAiRequester.AnalyzeSentiment(cleanedSpokenWords)
            });
        
        vcon.Analysis.Add(vconAnalysis);
        
        var vconJsonString = JsonConvert.SerializeObject(vcon);
        
        return vconJsonString;
    }
}