using System.Drawing;

namespace AoC_2023_CSharp.Models;

public class Section
{
    public Section(Point startPoint, Direction direction, int length, string[] rawDataLines)
    {
        StartPoint = startPoint;
        Direction = direction;
        Length = length;

        AverageTravelCost = GetAverageTravelCost(rawDataLines);
    }
    
    public Point StartPoint { get; }
    public Direction Direction { get; }

    public double AverageTravelCost { get; }
    
    public int Length { get; }
    
    private double GetAverageTravelCost(string[] rawLines)
    {
        var total = 0;
        
        switch (Direction)
        {
            case Direction.Uninitialized:
                break;
            
            case Direction.Right:
                for (var x = StartPoint.X; x < StartPoint.X + Length; x++)
                {
                    total += int.Parse(rawLines[StartPoint.Y][x].ToString());
                }

                return (double)total / Length;
            
            case Direction.Down:
                for (var y = StartPoint.Y; y < StartPoint.Y + Length; y++)
                {
                    total += int.Parse(rawLines[y][StartPoint.X].ToString());
                }

                return (double)total / Length;
            
            case Direction.Up:
                for (var y = StartPoint.Y; y > StartPoint.Y - Length; y--)
                {
                    total += int.Parse(rawLines[y][StartPoint.X].ToString());
                }

                return (double)total / Length;
                
            case Direction.Left:
                for (var x = StartPoint.X; x > StartPoint.X - Length; x--)
                {
                    total += int.Parse(rawLines[StartPoint.Y][x].ToString());
                }

                return (double)total / Length;
            
        }
        
        throw new ArgumentOutOfRangeException();
    }
}

public enum Direction
{
    Uninitialized,
    Right,
    Down, 
    Up,
    Left
}