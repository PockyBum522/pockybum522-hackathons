using System.Collections.Generic;
using System.Drawing;
using System.Reactive.Concurrency;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Aoc2023CSharp.Logic;
using Aoc2023CSharp.Models;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using ReactiveUI;
using Serilog.Core;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace Aoc2023CSharp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private Bitmap? _boardImage;
    
    [ObservableProperty] private int _windowWidth;
    [ObservableProperty] private int _windowHeight;
    
    private readonly ILogger _logger;
    private readonly LoggerConfiguration _loggerConfiguration;
    private readonly BeamRenderer _beamRenderer;
    private TextRenderer _textRenderer;

    public MainViewModel() { throw new NotImplementedException(); }     // This method being present keeps the designer happy

    [SupportedOSPlatform("windows")]
    public MainViewModel(LoggerConfiguration loggerConfiguration)
    {
        _loggerConfiguration = loggerConfiguration;

        _logger = loggerConfiguration
            .MinimumLevel.Information()
            .CreateLogger();

        _beamRenderer = new BeamRenderer(_logger);
        _textRenderer = new TextRenderer(_logger);
        
        RxApp.MainThreadScheduler.Schedule(async () => await RunBeams());
    }

    [SupportedOSPlatform("windows")]
    private async Task RunBeams()
    {
        var textToRender = RawData.ActualData01;
        
        var renderedBitmap = _textRenderer.GetBlankBoardBitmap(textToRender);
        BoardImage = AvaloniaImageHelper.ConvertToAvaloniaBitmap(renderedBitmap); 
        
        WindowWidth = (int)BoardImage.Size.Width;
        WindowHeight = (int)BoardImage.Size.Height;

        var allBeams = new List<Beam>();
        
        var dataLines = textToRender.Split(Environment.NewLine);
        var dataHeight = dataLines.Length;
        var dataWidth = dataLines[0].Length;

        var answerSpaces = new bool[dataWidth, dataHeight];
        var squareBeamVisitCountSpaces = new int[dataWidth, dataHeight];
        
        allBeams.Add(
            new Beam(-3, 0, BeamDirections.Right, textToRender.Split(Environment.NewLine), allBeams, answerSpaces, squareBeamVisitCountSpaces, null, _logger));

        await Task.Delay(3000);

        for (var x = 0; x < dataWidth; x++)
        {
            for (var y = 0; y < dataHeight; y++)
            {
                answerSpaces[x, y] = false;
            }
        }

        var steps = 0;
        
        while (AreAnyBeamsStillMoving(allBeams))
        {
            _beamRenderer.MoveAllBeamsOneStep(allBeams);

            renderedBitmap = _beamRenderer.RenderAllBeams(renderedBitmap, allBeams);

            renderedBitmap = _textRenderer.RenderTextOnImage(textToRender, renderedBitmap);

            BoardImage = AvaloniaImageHelper.ConvertToAvaloniaBitmap(renderedBitmap);

            RemoveUnmovingLasersFrom(allBeams);

            if (steps++ % 50 == 0)
                _logger.Information("Step count: {Steps}, allBeams.Count: {BeamsCount}", steps, allBeams.Count);
            
            await Task.Delay(1);
        }

        TallyEnergizedSquares(answerSpaces, squareBeamVisitCountSpaces, dataWidth, dataHeight);

        // TODO: Post on youtube
    }

    private void TallyEnergizedSquares(bool[,] answerSpaces, int[,] squareBeamVisitCountSpaces, int dataWidth,
        int dataHeight)
    {
        var energizedSquaresAnswer = (long)0;

        for (var x = 0; x < dataWidth; x++)
        {
            for (var y = 0; y < dataHeight; y++)
            {
                if (answerSpaces[x, y]) energizedSquaresAnswer++;
            }
        }

        var visitSpacesAnswer = 0;
        
        for (var x = 0; x < dataWidth; x++)
        {
            for (var y = 0; y < dataHeight; y++)
            {
                if (squareBeamVisitCountSpaces[x, y] > 0) visitSpacesAnswer++;
            }
        }
        
        _logger.Information("Energized squares count: {Count}", energizedSquaresAnswer);
        _logger.Information("Visit spaces count: {Count}", visitSpacesAnswer);
    }

    private void RemoveUnmovingLasersFrom(List<Beam> allBeams)
    {
        var beamsForRemoval = new List<Beam>();
        
        for (var i = 0; i < allBeams.Count; i++)
        {
            var beam = allBeams[i];

            if (beam.IsStopped)
                beamsForRemoval.Add(beam);
        }

        //_logger.Information("About to remove {Count} non-moving lasers", beamsForRemoval.Count);
        
        foreach (var beamForRemoval in beamsForRemoval)
        {
            allBeams.Remove(beamForRemoval);
        }
    }

    private bool AreAnyBeamsStillMoving(List<Beam> allBeams)
    {
        foreach (var beam in allBeams)
        {
            if (!beam.IsStopped) return true;
        }

        return false;
    }
}
