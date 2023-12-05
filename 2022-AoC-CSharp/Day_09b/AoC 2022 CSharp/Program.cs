using System.Drawing;
using AoC_2022_CSharp.Models;
using Serilog;
using Serilog.Data;

namespace AoC_2022_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.BuildLogger(); 
    
    public static void Main()
    {
        const int boardWidth = 900;
        const int boardHeight = 900;
        
        var rawLines = RawData.ActualData
            .Split(Environment.NewLine);

        var ropeBoard = InitializeRopeBoard(boardWidth, boardHeight);

        var ropeSegments = new List<Point>();
        
        // init tail segments
        for (var i = 0; i < 10; i++)
        {
            ropeSegments.Add(new Point(boardWidth / 2, boardHeight / 2));
        }


        var lineCount = 0;
        
        foreach (var line in rawLines)
        {
            Logger.Information("Executing line {CurrentLineNumber} of {AllLineCount}", lineCount++, rawLines.Length);
            
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

                UpdateSegmentsPosition(ropeBoard, ropeSegments, boardWidth, boardHeight);
                
                DrawNewBoard(ropeBoard, ropeSegments, boardWidth, boardHeight, true);
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

    private static void UpdateSegmentsPosition(BoardSquare[,] ropeBoard, List<Point> ropeSegments, int boardWidth, int boardHeight)
    {
        for (var i = 1; i < ropeSegments.Count; i++)
        {
            var segmentToMove = ropeSegments[i];
            var nextClosestSegmentToHead = ropeSegments[i - 1];
            
            if (segmentToMove.Y > nextClosestSegmentToHead.Y + 1 &&
                segmentToMove.X < nextClosestSegmentToHead.X - 1)
            {
                // segmentToMove is too far to the south

                Logger.Debug("Moving segment {SegmentIndex} north-east", i);

                var newY = segmentToMove.Y - 1;
                var newX = segmentToMove.X + 1;
                
                ropeSegments.Insert(i, new Point(newX, newY));
                ropeSegments.RemoveAt(i + 1);
            }
            else if (segmentToMove.Y > nextClosestSegmentToHead.Y + 1 &&
                     segmentToMove.X > nextClosestSegmentToHead.X + 1)
            {
                // segmentToMove is too far to the south

                Logger.Debug("Moving segment {SegmentIndex} north-west", i);
                
                var newY = segmentToMove.Y - 1;
                var newX = segmentToMove.X - 1;
                
                ropeSegments.Insert(i, new Point(newX, newY));
                ropeSegments.RemoveAt(i + 1);
            }
            else if (segmentToMove.Y < nextClosestSegmentToHead.Y - 1 &&
                     segmentToMove.X < nextClosestSegmentToHead.X - 1)
            {
                // segmentToMove is too far to the south

                Logger.Debug("Moving segment {SegmentIndex} south-east", i);

                var newY = segmentToMove.Y + 1;
                var newX = segmentToMove.X + 1;
                
                ropeSegments.Insert(i, new Point(newX, newY));
                ropeSegments.RemoveAt(i + 1);
            }
            else if (segmentToMove.Y < nextClosestSegmentToHead.Y - 1 &&
                     segmentToMove.X > nextClosestSegmentToHead.X + 1)
            {
                // segmentToMove is too far to the south

                Logger.Debug("Moving segment {SegmentIndex} south-west", i);

                var newY = segmentToMove.Y + 1;
                var newX = segmentToMove.X - 1;
                
                ropeSegments.Insert(i, new Point(newX, newY));
                ropeSegments.RemoveAt(i + 1);
            }
            else if (segmentToMove.Y > nextClosestSegmentToHead.Y + 1)
            {
                // segmentToMove is too far to the south

                Logger.Debug("Moving segment {SegmentIndex} north", i);

                var newY = segmentToMove.Y - 1;
                var newX = nextClosestSegmentToHead.X;
                
                ropeSegments.Insert(i, new Point(newX, newY));
                ropeSegments.RemoveAt(i + 1);
            }
            else if (segmentToMove.X < nextClosestSegmentToHead.X - 1)
            {
                // segmentToMove is too far to the left

                Logger.Debug("Moving segment {SegmentIndex} right", i);
                
                var newY = nextClosestSegmentToHead.Y;
                var newX = segmentToMove.X + 1;
                
                ropeSegments.Insert(i, new Point(newX, newY));
                ropeSegments.RemoveAt(i + 1);
            }
            else if (segmentToMove.Y < nextClosestSegmentToHead.Y - 1)
            {
                // segmentToMove is too far to the north

                Logger.Debug("Moving segment {SegmentIndex} south", i);

                var newY = segmentToMove.Y + 1;
                var newX = nextClosestSegmentToHead.X;
                
                ropeSegments.Insert(i, new Point(newX, newY));
                ropeSegments.RemoveAt(i + 1);
            }
            else if (segmentToMove.X > nextClosestSegmentToHead.X + 1)
            {
                // segmentToMove is too far to the right

                Logger.Debug("Moving segment {SegmentIndex} left", i);

                var newY = nextClosestSegmentToHead.Y;
                var newX = segmentToMove.X - 1;
                
                ropeSegments.Insert(i, new Point(newX, newY));
                ropeSegments.RemoveAt(i + 1);
            }
            
            Logger.Debug("Updated segments a step, last updated segment was {Index}:", i);
            DrawNewBoard(ropeBoard, ropeSegments, boardWidth, boardHeight, true);
        }
        
        Logger.Debug("Updated segments: {@Segments}", ropeSegments);
        
        ropeBoard[ropeSegments[^1].X, ropeSegments[^1].Y].HasTailBeenHere = true;
    }

    private static void DrawNewBoard(
        BoardSquare[,] ropeBoard, 
        List<Point> ropeSegments, 
        int width,
        int height, 
        bool showTailAndHead = true)
    {
        var boardStrings = new string[height + 1];

        var tailSegmentPositions = new List<Point>();
        
        for (var y = 0; y < height; y++)
        {
            var currentRowString = "";
            
            for (var x = 0; x < width; x++)
            {
                if (showTailAndHead)
                {
                    // Check that we haven't drawn a segment at this location yet
                    if (TailSegmentPositionsHasMatchingPoint(x, y, tailSegmentPositions)) continue;
                    
                    if (x == ropeSegments[0].X &&
                        y == ropeSegments[0].Y)
                    {
                        currentRowString += 'H';
                        
                        // Log that we've drawn a segment here so we don't draw more here
                        tailSegmentPositions.Add(new Point(x, y));
                        
                        continue;
                    }

                    // Draw rest of segments
                    for (int i = 1; i < ropeSegments.Count - 1; i++)
                    {
                        if (TailSegmentPositionsHasMatchingPoint(x, y, tailSegmentPositions)) continue;

                        if (x == ropeSegments[i].X &&
                            y == ropeSegments[i].Y)
                        {
                            currentRowString += i;
                            
                            // Log that we've drawn a segment here so we don't draw more here
                            tailSegmentPositions.Add(new Point(x, y));
                        }   
                    }
                    
                    // Draw tail
                    if (x == ropeSegments[^1].X &&
                        y == ropeSegments[^1].Y)
                    {
                        if (TailSegmentPositionsHasMatchingPoint(x, y, tailSegmentPositions)) continue;

                        tailSegmentPositions.Add(new Point(x, y));

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
                    // Check that we haven't drawn a segment at this location yet
                    if (TailSegmentPositionsHasMatchingPoint(x, y, tailSegmentPositions)) continue;
                    
                    currentRowString += '.';
                }    
            }

            tailSegmentPositions.Clear();
            
            boardStrings[y] = currentRowString;
        }

        Logger.Debug("");
        
        foreach (var line in boardStrings)
        {
            Logger.Debug("{Line}", line);    
        }

        Logger.Debug("");
        Logger.Debug("");
    }

    private static bool TailSegmentPositionsHasMatchingPoint(int x, int y, List<Point> tailSegmentPositions)
    {
        foreach (var segmentPosition in tailSegmentPositions)
        {
            if (segmentPosition.X == x &&
                segmentPosition.Y == y)
                return true;
        }

        return false;
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