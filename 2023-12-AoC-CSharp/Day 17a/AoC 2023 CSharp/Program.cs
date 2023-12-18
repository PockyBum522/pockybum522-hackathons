using System.Drawing;
using AoC_2023_CSharp.Models;

namespace AoC_2023_CSharp;

internal static class Program
{
    // ReSharper disable once InconsistentNaming because it's less annoying this way
    private static readonly ILogger _logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Debug()
            .CreateLogger(); 
 
    private static readonly Stopwatch ElapsedTotal = new();
    
    public static async Task Main()
    {
        ElapsedTotal.Start();
        _logger.Information("Starting!");
        
        var rawData = RawData.SampleData01;
        
        var rawLines = rawData.Split(Environment.NewLine);

        var currentPosition = new Point(0, 0);
        
        var boardWidth = rawLines[0].Length;
        var boardHeight = rawLines.Length;
        
        var endPosition = new Point(boardWidth, boardHeight);
        
        var answerTotal = 0;

        var sections = new List<Section>();
        
        for (var x = 0; x < boardWidth; x++)
        {
            for (var y = 0; y < boardHeight; y++)
            {
                // At each position
                AddAllPossibleSections(x, y, sections, rawLines);
            }   
        }

        var filteredSections = new List<Section>();
        
        foreach (var section in sections)
        {
            if (section.AverageTravelCost < 2)
            {
                // _logger.Debug("Section at x:{X}, y:{Y} with length {Length} in the {Direction} direction, has average travel cost of {AverageCost}",
                //     section.StartPoint.X, section.StartPoint.Y, section.Length, section.Direction, section.AverageTravelCost);
                
                 filteredSections.Add(section);
            }
        }

        var overlayedRawData = OverlaySectionsOnRawData(filteredSections, rawLines);
        
        LogRawData(overlayedRawData);
        
        _logger.Information("{FormattedTimeString}", StopwatchHelper.GetStopwatchFinalTimes(ElapsedTotal));
        _logger.Information("Answer: {AnswerTotal}", answerTotal);
        
        // Make sure if we log on other threads right before the program ends, we can see it
        await Log.CloseAndFlushAsync();
        await Task.Delay(500);
    }

    private static string[] OverlaySectionsOnRawData(List<Section> filteredSections, string[] rawData)
    {
        foreach (var section in filteredSections)
        {
            
            _logger.Debug("Section at x:{X}, y:{Y} with length {Length} in the {Direction} direction, has average travel cost of {AverageCost}",
                section.StartPoint.X, section.StartPoint.Y, section.Length, section.Direction, section.AverageTravelCost);
            
            switch (section.Direction)
            {
                case Direction.Uninitialized:
                    break;
                
                case Direction.Right:
                    for (var x = 0; x < section.StartPoint.X + section.Length; x++)
                    {
                        rawData[section.StartPoint.Y] = ReplaceCharacterInStringAt(rawData[section.StartPoint.Y], x, '>');
                    }
                    break;
                
                case Direction.Down:
                    for (var y = section.StartPoint.Y; y < section.StartPoint.Y + section.Length; y++)
                    {
                        rawData[y] = ReplaceCharacterInStringAt(rawData[y], section.StartPoint.X , 'v');
                    }
                    break;
                
                case Direction.Up:
                    for (var y = section.StartPoint.Y; y > section.StartPoint.Y - section.Length; y--)
                    {
                        rawData[y] = ReplaceCharacterInStringAt(rawData[y], section.StartPoint.X , '^');
                    }
                    break;
                
                case Direction.Left:
                    for (var x = section.StartPoint.X; x > section.StartPoint.X - section.Length; x--)
                    {
                        rawData[section.StartPoint.Y] = ReplaceCharacterInStringAt(rawData[section.StartPoint.Y], x, '<');
                    }
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return rawData;
    }

    private static string ReplaceCharacterInStringAt(string original, int x, char newChar)
    {
        var returnString = "";

        for (var i = 0; i < original.Length; i++)
        {
            if (i != x)
            {
                returnString += original[i];
                
                continue;
            }

            returnString += newChar;
        }

        return returnString;
    }

    private static void AddAllPossibleSections(int x, int y, List<Section> sections, string[] rawLines)
    {
        if (y - 1 > 0)
        {
            sections.Add(new Section(
                new Point(x, y), Direction.Up, 1, rawLines));    
        }

        if (y - 2 > 0)
        {
            sections.Add(new Section(
                new Point(x, y), Direction.Up, 2, rawLines));    
        }

        if (y - 3 > 0)
        {
            sections.Add(new Section(
                new Point(x, y), Direction.Up, 3, rawLines));    
        }

        if (y + 1 < rawLines.Length)
        {
            sections.Add(new Section(
                new Point(x, y), Direction.Down, 1, rawLines));    
        }

        if (y + 2 < rawLines.Length)
        {
            sections.Add(new Section(
                new Point(x, y), Direction.Down, 2, rawLines));    
        }

        if (y + 3 < rawLines.Length)
        {
            sections.Add(new Section(
                new Point(x, y), Direction.Down, 3, rawLines));    
        }

        if (x + 1 < rawLines[0].Length)
        {
            sections.Add(new Section(
                new Point(x, y), Direction.Right, 1, rawLines));    
        }
        
        if (x + 2 < rawLines[0].Length)
        {
            sections.Add(new Section(
                new Point(x, y), Direction.Right, 2, rawLines));    
        }
        
        if (x + 3 < rawLines[0].Length)
        {
            sections.Add(new Section(
                new Point(x, y), Direction.Right, 3, rawLines));    
        }

        if (x - 1 > 0)
        {
            sections.Add(new Section(
                new Point(x, y), Direction.Left, 1, rawLines));    
        }
        
        if (x - 2 > 0)
        {
            sections.Add(new Section(
                new Point(x, y), Direction.Left, 2, rawLines));    
        }
        
        if (x - 3 > 0)
        {
            sections.Add(new Section(
                new Point(x, y), Direction.Left, 3, rawLines));    
        }
    }

    private static void LogRawData(string[] rawData)
    {
        //var rawLines = rawData.Split(Environment.NewLine);
        
        _logger.Debug("");

        foreach (var line in rawData)
        {
            _logger.Debug("{Line}", line);    
        }
    }
}
