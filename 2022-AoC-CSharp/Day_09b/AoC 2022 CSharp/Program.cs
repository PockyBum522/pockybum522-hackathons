using System.Drawing;
using AoC_2022_CSharp.Models;
using Serilog;

namespace AoC_2022_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.BuildLogger(); 
    
    public static void Main()
    {
        const int boardWidth = 20;
        const int boardHeight = 20;
        
        var rawLines = RawData.SampleData01
            .Split(Environment.NewLine);

        var ropeBoard = InitializeRopeBoard(boardWidth, boardHeight);

        var ropeSegments = new List<Point>();
        
        // init tail segments
        for (var i = 0; i < 11; i++)
        {
            ropeSegments.Add(new Point(boardWidth / 2, boardHeight / 2));
        }

        foreach (var line in rawLines)
        {
            var direction = line.Split(' ')[0];
            var distance = line.Split(' ')[1];
            
            DrawNewBoard(ropeBoard, ropeSegments, boardWidth, boardHeight);
            
            for (var i = 0; i < int.Parse(distance); i++)
            {
                Logger.Debug("About to apply one step of instruction: {RawLine}", line);
                
                switch (direction)
                {
                    case "U":
                        ropeSegments[0] = new Point(ropeSegments[0].X, ropeSegments[0].Y - 1);
                        break;
                    
                    case "R":
                        ropeSegments[0] = new Point(ropeSegments[0].X + 1, ropeSegments[0].Y);
                        break;
                    
                    case "D":
                        ropeSegments[0] = new Point(ropeSegments[0].X, ropeSegments[0].Y + 1);
                        break;
                    
                    case "L":
                        ropeSegments[0] = new Point(ropeSegments[0].X - 1, ropeSegments[0].Y);
                        break;
                }

                UpdateSegmentsPosition(ropeBoard, ref ropeSegments, boardWidth, boardHeight);
                
                //DrawNewBoard(ropeBoard, headPosition, ropeSegments, boardWidth, boardHeight);
            }
        }
        
        DrawNewBoard(ropeBoard, ropeSegments, boardWidth, boardHeight, false);

        var answer = CalculateHowManySquaresTailVisited(ropeBoard, boardWidth, boardHeight);
        
        Logger.Information("Answer: {Answer}", answer);
    }

    private static int CalculateHowManySquaresTailVisited(BoardSquare[,] ropeBoard, int boardWidth, int boardHeight)
    {
        var totalSquares = 0;
        
        for (var y = 0; y < boardHeight; y++)
        {
            for (var x = 0; x < boardWidth; x++)
            {
                if (ropeBoard[x, y].HasTailBeenHere)
                    totalSquares++;
            }
        }

        return totalSquares;
    }

    private static void UpdateSegmentsPosition(BoardSquare[,] ropeBoard, ref List<Point> ropeSegments, int boardWidth, int boardHeight)
    {
        for (var i = 1; i < ropeSegments.Count; i++)
        {
            var previousSegment = ropeSegments[i - 1];
            var thisSegment = ropeSegments[i];
            
            // Check if this segment is not right next to previous segment
            if (previousSegment.X < thisSegment.X - 1)
            {
                // Prebious is one space away from this to the left
                thisSegment.X--;
                thisSegment.Y = previousSegment.Y;
            }
        
            if (previousSegment.X > thisSegment.X + 1)
            {
                // Head is one space away from tail to the right
                thisSegment.X++;
                thisSegment.Y = previousSegment.Y;
            }
        
            if (previousSegment.Y < thisSegment.Y - 1)
            {
                // Head is one space away from tail above
                thisSegment.X = previousSegment.X;
                thisSegment.Y--;
            }
        
            if (previousSegment.Y > thisSegment.Y + 1)
            {
                // Head is one space away from tail below
                thisSegment.X = previousSegment.X;
                thisSegment.Y++;
            }
        }
        
        ropeBoard[ropeSegments[10].X, ropeSegments[10].Y].HasTailBeenHere = true;
    }

    private static void DrawNewBoard(
        BoardSquare[,] ropeBoard, 
        List<Point> ropeSegments, 
        int width,
        int height, 
        bool showTailAndHead = true)
    {
        var boardStrings = new string[height + 1];
        
        for (var y = 0; y < height; y++)
        {
            var currentRowString = "";
            
            for (var x = 0; x < width; x++)
            {
                if (showTailAndHead)
                {
                    if (x == ropeSegments[0].X &&
                        y == ropeSegments[0].Y)
                    {
                        currentRowString += 'H';
                        continue;
                    }

                    for (int i = 1; i < ropeSegments.Count - 1; i++)
                    {
                        if (x == ropeSegments[i].X &&
                            y == ropeSegments[i].Y)
                        {
                            currentRowString += i;
                            break;
                        }   
                    }
                
                    if (x == ropeSegments[10].X &&
                        y == ropeSegments[10].Y)
                    {
                        currentRowString += 'T';
                        continue;
                    }
                }
                
                if (ropeBoard[x, y].HasTailBeenHere)
                {
                    currentRowString += '#';
                }
                else
                {
                    currentRowString += '.';
                }    
            }

            boardStrings[y] = currentRowString;
        }

        foreach (var line in boardStrings)
        {
            Console.WriteLine(line);    
        }

        Console.WriteLine();
        Console.WriteLine();
    }

    private static BoardSquare[,] InitializeRopeBoard(int width, int height)
    {
        var ropeBoard = new BoardSquare[width + 1, height + 1];
        
        for (var y = 0; y <= height; y++)
        {
            for (var x = 0; x <= width; x++)
            {
                ropeBoard[x, y] = new BoardSquare();    
            }
        }

        return ropeBoard;
    }
}