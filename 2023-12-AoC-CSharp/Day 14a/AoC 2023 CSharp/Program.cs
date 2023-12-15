using Serilog;
using Serilog.Events;

namespace AoC_2023_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Information()
            .CreateLogger(); 
    
    public static void Main()
    {
        Logger.Information("Starting!");
        
        var rawLines = RawData.ActualData01
            .Split(Environment.NewLine);
        
        string[] lastRawLines;

        LogAllRawLines(rawLines);

        do
        {
            lastRawLines = (string[])rawLines.Clone();
            
            MoveAllOsUpOne(rawLines);

            LogAllRawLines(rawLines);
        } 
        while (!HasSameCharacters(lastRawLines, rawLines));

        var answerTotal = ScoreAllRows(rawLines);
        
        Logger.Information("Answer: {AnswerTotal}", answerTotal);
        
        // Make sure if we log on other threads right before the program ends, we can see it
        Log.CloseAndFlush();
        Task.Delay(2000);
    }

    private static int ScoreAllRows(string[] rawLines)
    {
        var total = 0;

        var rowScoreValue = rawLines.Length;
        
        for (var i = 0; i < rawLines.Length; i++)
        {
            foreach (var character in rawLines[i])
            {
                if (character != 'O') continue;
                
                Logger.Debug("In row: {Row}, found O", i);
                Logger.Verbose("Adding: {RowScore}", rowScoreValue);
                
                total += rowScoreValue;
            }

            Logger.Debug("After row: {Row}, total is: {Total}", i, total);
            
            rowScoreValue--;
            
            Logger.Debug("");
        }

        return total;
    }

    private static bool HasSameCharacters(string[] lastRawLines, string[] rawLines)
    {
        for (var y = 0; y < lastRawLines.Length; y++)
        {
            var lastRawLine = lastRawLines[y];
            var rawLine = rawLines[y];

            if (lastRawLine != rawLine)
                return false;
        }
        
        return true;
    }

    private static void LogAllRawLines(string[] rawLines)
    {
        foreach (var line in rawLines)
        {
            Logger.Debug("{Line}", line);
        }
        
        Logger.Debug("");
        Logger.Debug("");
    }

    private static void MoveAllOsUpOne(string[] rawLines)
    {
        for (var y = 1; y < rawLines.Length; y++)
        {
            for (var x = 0; x < rawLines[y].Length; x++)
            {
                if (rawLines[y][x] != 'O') continue;
                
                if (rawLines[y - 1][x] != '.') continue;

                rawLines[y - 1] = PlaceCharIn(rawLines[y - 1], x, 'O');
                rawLines[y] = PlaceCharIn(rawLines[y], x, '.');
            }
        }
    }

    private static string PlaceCharIn(string line, int x, char charToInsert)
    {
        var returnString = "";
        
        for (var i = 0; i < line.Length; i++)
        {
            if (i == x)
            {
                returnString += charToInsert;

                continue;
            }
            
            returnString += line[i];
        }

        return returnString;
    }
}
