using System.Diagnostics;
using OpenAI.Chat;
using ShrineBackendServer.Models;

namespace ShrineBackendServer;

public class SpeechRecognizer
{
    private static bool _speechRecognizerDebug = false;
    
    public static async Task StartSpeechRecognitionLoop()
    {
        Console.WriteLine("Starting Speech Streaming Test");

        var hasRecognitionInitialized = false;
        var hasRecognitionStarted = false;

        var lastWordsStartSection = "afwiegha4hgawebhsdifhaw";
        var fullSpokenWords = "";
        
        var speechRecognitionProcessInfo = new ProcessStartInfo()
        {
            FileName = "/media/secondary/repos/streaming-sensevoice/venv/bin/python3",
            Arguments = "-u /media/secondary/repos/streaming-sensevoice/realtime.py",
            WorkingDirectory = "/media/secondary/repos/streaming-sensevoice",
            RedirectStandardOutput = true,
            UseShellExecute = false
        };
        
        var speechRecognitionProcess = new Process()
        {
            StartInfo = speechRecognitionProcessInfo
        };
        
        speechRecognitionProcess.Start();

        while (true)
        {
            await Task.Delay(200);

            var newWordsLine = speechRecognitionProcess.StandardOutput.ReadLine();
            
            if (string.IsNullOrWhiteSpace(newWordsLine)) continue;

            if (newWordsLine.StartsWith("text: ") && !hasRecognitionInitialized)
            {
                hasRecognitionInitialized = true;
                Console.WriteLine("Recognition initialized");
            } 
            
            if (!hasRecognitionInitialized) continue;
            
            if (HttpServer.CoinPlacedTime >= DateTimeOffset.Now.Subtract(TimeSpan.FromSeconds(5))) continue;
            
            if (newWordsLine.StartsWith("text: ") && !hasRecognitionStarted)
            {
                hasRecognitionStarted = true;
                Console.WriteLine("Recognition has begun, they should start speaking");
            } 
            
            // Make sure we only operate on lines that contain the speech we want
            if (!newWordsLine.StartsWith("text: ")) continue;
            
            newWordsLine = newWordsLine.Replace("text: ", "");
            
            if (_speechRecognizerDebug)
                Console.WriteLine($"New words line raw: {newWordsLine}");

            if (!newWordsLine.StartsWith(lastWordsStartSection))
            {
                lastWordsStartSection = newWordsLine;
                
                // And make all of the start section words into events
                foreach (var newWord in lastWordsStartSection.Split(" "))
                {
                    if (_speechRecognizerDebug)
                        Console.WriteLine($"MAKING AND SENDING NEW WORD: {newWord}");

                    if (string.IsNullOrWhiteSpace(newWord)) continue;

                    fullSpokenWords += newWord + " ";;
                        
                    HttpServer.Events.Add(
                        new Event()
                        {
                            Type = "NewWordSpoken", 
                            Data = newWord
                        });
                }
            }
            else
            {
                // if (newLine DOES start with lastWordsStartSection)
                    // Just means we've gotten more words in the same speech
                    // Do a diff that removes lastWordsStartSection from newLine
                    // Then send whatever's left in the newLine
                    // Then add newLine to lastWordsStartSection so we can check for new words on next line

                newWordsLine = newWordsLine.Replace(lastWordsStartSection, "");
                
                // Make whatever's left in newLine now into events
                foreach (var newWord in newWordsLine.Split(" "))
                {
                    if (string.IsNullOrWhiteSpace(newWord)) continue;
                    
                    if (_speechRecognizerDebug)
                        Console.WriteLine($"MAKING AND SENDING NEW WORD: {newWord}");
                    
                    fullSpokenWords += newWord + " ";
                    
                    HttpServer.Events.Add(
                        new Event()
                        {
                            Type = "NewWordSpoken", 
                            Data = newWord
                        });
                }
                
                lastWordsStartSection += newWordsLine;
            }

            //if (_speechRecognizerDebug)
            //Console.WriteLine(fullSpokenWords + '\n');
        }

        // ReSharper disable once FunctionNeverReturns
    }
}