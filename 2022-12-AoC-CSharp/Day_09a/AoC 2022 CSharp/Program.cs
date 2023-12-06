using System.Drawing;
using AoC_2022_CSharp.Models;
using Serilog;

namespace AoC_2022_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.BuildLogger(); 
    
    public static void Main()
    {
        const int boardWidth = 1000;
        const int boardHeight = 1000;
        
        var rawLines = RawData.ActualData
            .Split(Environment.NewLine);

        var ropeBoard = InitializeRopeBoard(boardWidth, boardHeight);

        var headPosition = new Point(boardWidth / 2, boardHeight / 2);
        var tailPosition = new Point(boardWidth / 2, boardHeight / 2);
        
        foreach (var line in rawLines)
        {
            var direction = line.Split(' ')[0];
            var distance = line.Split(' ')[1];
            
            //DrawNewBoard(ropeBoard, headPosition, tailPosition, boardWidth, boardHeight);
            
            for (var i = 0; i < int.Parse(distance); i++)
            {
                Logger.Debug("About to apply one step of instruction: {RawLine}", line);
                
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

                UpdateTailPosition(ropeBoard, headPosition, ref tailPosition, boardWidth, boardHeight);
                
                //DrawNewBoard(ropeBoard, headPosition, tailPosition, boardWidth, boardHeight);
            }
        }
        
        DrawNewBoard(ropeBoard, headPosition, tailPosition, boardWidth, boardHeight, false);

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

    private static void UpdateTailPosition(BoardSquare[,] ropeBoard, Point headPosition, ref Point tailPosition, int boardWidth, int boardHeight)
    {
        // For brevity
        var headX = headPosition.X;
        var headY = headPosition.Y;
        
        var tailX = tailPosition.X;
        var tailY = tailPosition.Y;
        
        // Check if head is not right next to tail
        if (headX < tailX - 1)
        {
            // Head is one space away from tail to the left
            tailPosition.X = tailX - 1;
            tailPosition.Y = headY;
        }
        
        if (headX > tailX + 1)
        {
            // Head is one space away from tail to the right
            tailPosition.X = tailX + 1;
            tailPosition.Y = headY;
        }
        
        if (headY < tailY - 1)
        {
            // Head is one space away from tail above
            tailPosition.X = headX;
            tailPosition.Y = tailY - 1;
        }
        
        if (headY > tailY + 1)
        {
            // Head is one space away from tail below
            tailPosition.X = headX;
            tailPosition.Y = tailY + 1;
        }
        
        ropeBoard[tailPosition.X, tailPosition.Y].HasTailBeenHere = true;
    }

    private static void DrawNewBoard(
        BoardSquare[,] ropeBoard, 
        Point headPosition, 
        Point tailPosition, 
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
                    if (x == headPosition.X &&
                        y == headPosition.Y)
                    {
                        currentRowString += 'H';
                        continue;
                    }
                
                    if (x == tailPosition.X &&
                        y == tailPosition.Y)
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