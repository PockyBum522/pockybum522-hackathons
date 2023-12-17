using System.Drawing;

namespace Aoc2023CSharp.Models;

public class StartPoint
{
    public StartPoint(Point point, BeamDirections direction)
    {
        Point = point;
        Direction = direction;
    }
    
    public Point Point { get;  }
    
    public BeamDirections Direction { get; }
}