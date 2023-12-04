using System.Drawing;

namespace AoC_2023_CSharp.Models;

public class FoundNumber
{
    public Point AsteriskPosition { get; set; } = new Point(-1, -1);
    
    public Point NumberPosition { get; set; } = new Point(-1, -1);
    
    public string Number { get; set; } = "";
}
