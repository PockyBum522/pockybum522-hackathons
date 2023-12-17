using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Versioning;

namespace Aoc2023CSharp.Logic;

public class TextRenderer
{
    public static int TextSize => 7; 
    public static string AdditionalHorizontalCharacters => "    "; 
    
    public static double StartX => 50;
    public static double StartY => 40;
    
    public static double RenderedWidth { get; private set; }
    public static double RenderedHeight { get; private set; }
    
    private readonly ILogger _logger;
    
    public TextRenderer(ILogger logger)
    {
        _logger = logger;
    }
    
    [SupportedOSPlatform("windows")]
    public Bitmap GetBlankBoardBitmap(string stringToRender)
    {
        var stringAsLines = GetFormattedStringLines(stringToRender);
        
        RenderedWidth = stringAsLines[0].Length * 0.85d * TextSize;
        RenderedHeight = stringAsLines.Length * 2.84d * TextSize;
        
        _logger.Debug("Image width: {Width}, height: {Height}", RenderedWidth, RenderedHeight);
        
        var image = new Bitmap((int)RenderedWidth, (int)RenderedHeight);
        
        var graphics = Graphics.FromImage(image);
        
        var backColor = new SolidBrush(Color.Black);
        
        graphics.FillRectangle(backColor, 0, 0, (int)RenderedWidth, (int)RenderedHeight);
        
        return image;
    }
    
    [SupportedOSPlatform("windows")]
    public Bitmap RenderTextOnImage(string stringToRender, Bitmap image)
    {
        var stringAsLines = GetFormattedStringLines(stringToRender);
        
        var graphics = Graphics.FromImage(image);
        
        var verticalLineSpacing = BeamRenderer.VerticalBoardSpacing;
        var yPosition = StartY;
        
        foreach (var line in stringAsLines)
        {
            yPosition += verticalLineSpacing;
            
            DrawText((int)StartX, (int)yPosition, line, graphics);
        }
        
        return image;
    }

    private string[] GetFormattedStringLines(string rawData)
    {
        rawData = rawData.Replace(".", " ");
        
        var splitLines = rawData.Split(Environment.NewLine);

        var returnLines = new List<string>();
        
        // Add spaces between letters
        foreach (var line in splitLines)
        {
            var thisLine = "";
            
            foreach (var character in line)
            {
                thisLine += character;
                thisLine += AdditionalHorizontalCharacters;
            }

            returnLines.Add(thisLine);
        }

        return returnLines.ToArray();
    }

    [SupportedOSPlatform("windows")]
    private void DrawText(int startX, int startY, string toDraw, Graphics graphics)
    {
        var font = new Font("Cascadia Mono SemiBold", TextSize);
        
        var textPoint = new PointF(startX, startY);
        
        var foreColor = new SolidBrush(Color.LawnGreen);
        
        graphics.DrawString(toDraw, font, foreColor, textPoint);
    }
}