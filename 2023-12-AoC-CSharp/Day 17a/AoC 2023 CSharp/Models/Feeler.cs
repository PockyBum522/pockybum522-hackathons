using System.Drawing;

namespace AoC_2023_CSharp.Models;

public class Feeler
{
    public Feeler(Point startPoint, Point destination)
    {
        _destination = destination;
        StartPoint = startPoint;
    }
    
    public Point StartPoint { get; }

    public List<Point> Segments { get; }

    public bool IsSearching { get; } = true;
    
    private readonly Point _destination;

    public void FindLowestNextX(int howManyToConsider)
    {
        // Map all sections going horizontally and vertically
        
        // Of length 1, 2, or 3
        
        // That cost less than x per square on average
        
        // Rate each one with the average cost of travelling on it
        
        // Find the lowest one
        
        // Move out along the others from that one via lowest avg cost sections until you hit the start point
        
        // Move out along the others from that one via lowest avg cost sections until you hit the end point
        
        // Connect those two, calculate cost
        
        
        
        // Might need something that can establish a start and end point on the route then check other paths that deviate to see if they're cheaper
        // Move route if so
    }
}