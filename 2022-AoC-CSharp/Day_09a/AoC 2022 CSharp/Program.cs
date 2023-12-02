using System.Drawing;
using AoC_2022_CSharp.Models;
using Serilog;

namespace AoC_2022_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.BuildLogger(); 
    
    public static void Main()
    {
        var rawLines = RawData.ActualData
            .Split(Environment.NewLine);

        var ropeBoard = InitializeRopeBoard(6, 5);

        var headPosition = new Point(4, 0);
        var tailPosition = new Point(4, 0);
        
        foreach (var line in rawLines)
        {
            var direction = line.Split(' ')[0];
            var distance = line.Split(' ')[1];

            for (var i = 0; i < int.Parse(distance); i++)
            {
                switch (direction)
                {
                    case "U":
                        headPosition = new Point(headPosition.X, headPosition.Y - 1);
                        break;
                    
                    case "R":
                        headPosition = new Point(headPosition.X + 1, headPosition.Y);
                        break;
                    
                    case "D":
                        headPosition = new Point(headPosition.X, headPosition.Y + 1);
                        break;
                    
                    case "L":
                        headPosition = new Point(headPosition.X - 1, headPosition.Y);
                        break;
                }

                DrawNewBoard(ropeBoard, headPosition, tailPosition, 6, 5);
            }
        }
        
        //Logger.Debug("Answer: {NumberTrees}", answerTotal);
    }

    private static void DrawNewBoard(BoardSquare[,] ropeBoard, Point headPosition, Point tailPosition, int width, int height)
    {
        for (var y = 0; y <= height; y++)
        {
            for (var x = 0; x <= width; x++)
            {
                if (BoardSquare)    
            }
        }
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