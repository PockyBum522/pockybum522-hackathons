using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Aoc2023CSharp.Models;
using Aoc2023CSharp.ViewModels;

namespace Aoc2023CSharp.Logic;

public class BeamRenderer
{
    public static double VerticalBoardSpacing => 
        TextRenderer.TextSize * 2.7d;
    
    public static double HorizontalBoardSpacing => 
        TextRenderer.TextSize * TextRenderer.AdditionalHorizontalCharacters.Length * 1.0075d;

    public static double YStartOffset;
    public static double XStartOffset;

    private Color _beamColor = Color.FromArgb(50, 255, 0, 0);
    private ILogger _logger;

    static BeamRenderer()
    {
        var halfCharacterHeight = 5.5;
        
        YStartOffset = TextRenderer.StartY + halfCharacterHeight + 18;  
        
        XStartOffset = TextRenderer.StartX + 4; // + Half a character width
    }

    public BeamRenderer(ILogger logger)
    {
        _logger = logger;
    }
    
    public void MoveAllBeamsOneStep(List<Beam> allBeams)
    {
        _logger.Debug("Moving all beams one step!");
        
        for (var i = 0; i < allBeams.Count; i++)
        {
            var beam = allBeams[i];

            LogAllBeamsStatus(allBeams);
            
            // _logger.Debug("In for, on beam {Index} - Going:{Direction} X:{CurrentX}, Y:{CurrentY}",
            //     beam.ThisBeamIndex, beam.CurrentDirection, beam.CurrentHeadX, beam.CurrentHeadY);
            
            if (!beam.IsStopped)
            {
                _ = beam.TravelOneStep();
            }
        }
    }

    private void LogAllBeamsStatus(List<Beam> allBeams)
    {
        _logger?.Debug("");

        foreach (var beam in allBeams)
        {
            _logger?.Debug("Beam {Index} - Is travelling: {Direction} and IsStopped: {Stopped}",
                beam.ThisBeamIndex, beam.CurrentDirection, beam.IsStopped);
        }
    }

    [SupportedOSPlatform("windows")]
    public Bitmap RenderAllBeams(Bitmap image, List<Beam> allBeams)
    {
        var pen = new Pen(_beamColor, 0.05f);
        
        var graphics = Graphics.FromImage(image);
        
        foreach (var beam in allBeams)
        {
            if (beam.IsStopped) continue;
            
            graphics.DrawLine(pen, beam.RenderFrom, beam.RenderTo);    
        }

        return image;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="renderedText">Original raw data string from Advent Of Code</param>
    /// <param name="image">Image to render beam onto</param>
    [SupportedOSPlatform("windows")]
    public Bitmap RenderBeamTestGridOn(string renderedText, Bitmap image)
    {
        var yPositionTop = 0;
        var yPositionBottom = (int)(TextRenderer.RenderedHeight + 5);
        
        var graphics = Graphics.FromImage(image);

        var textWidthInCharacters = renderedText.Split(Environment.NewLine)[0].Length;
        
        // Draw vertical lines
        var i = 0;

        var xPosition = TextRenderer.StartX + 4; // + Half a character width
        
        while (i < textWidthInCharacters)
        {
            var topPoint = new Point((int)xPosition, yPositionTop);
            var bottomPoint = new Point((int)xPosition, yPositionBottom);
        
            var pen = new Pen(_beamColor, 0.05f);
            
            graphics.DrawLine(pen, topPoint, bottomPoint);
        
            xPosition += HorizontalBoardSpacing;
            
            i++;
        }
        
        // Draw horizontal lines
        i = 0;
        
        var yPosition = YStartOffset; 
        
        var xPositionLeft = 0;
        var xPositionRight = (int)(TextRenderer.RenderedWidth + 5);
        
        while (i < textWidthInCharacters)
        {
            var topPoint = new Point(xPositionLeft, (int)yPosition);
            var bottomPoint = new Point(xPositionRight, (int)yPosition);
        
            var pen = new Pen(_beamColor, 0.05f);
            
            graphics.DrawLine(pen, topPoint, bottomPoint);
        
            yPosition += VerticalBoardSpacing;
            
            i++;
        }

        return image;
    }
}