using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
    
    private readonly Stopwatch _elapsedTotal = new();
    
    private readonly int _maxThreads = 31;

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
        _elapsedTotal.Start();
        _logger.Information("Starting!");
        
        var textToRender = RawData.ActualData01;
        
        var startPoints = new List<StartPoint>();
        
        var dataLines = textToRender.Split(Environment.NewLine);
        var dataHeight = dataLines.Length;
        var dataWidth = dataLines[0].Length;
        
        GetAllStartPoints(dataWidth, startPoints, dataHeight);
        
        var startPointsCount = 1;
        
        var tasksList = new ConcurrentBag<Task<long>>();
        var answersList = new ConcurrentBag<long>();;
        
        foreach (var startPoint in startPoints)
        {        
            //_logger.Information("On startpoint: {ThisCount} with a total of: {TotalCount} startpoints", startPointsCount++, startPoints.Count);

            var nextTask = Task.Run(() => RunStartPoint(textToRender, dataWidth, dataHeight, startPoint));
            tasksList.Add(nextTask);

            // _logger.Information("tasksList.Count: {TaskCount}", tasksList.Count);
            
            if (tasksList.Count < _maxThreads) continue;
            
            await Task.Delay(1); // Make sure tasksList is done adding before we check it
            
            var runningTasks = GetRunningTasksCount(tasksList);

            // _logger.Information("runningTasks: {TaskCount}", runningTasks);

            while (runningTasks > _maxThreads - 1)
            {
                await Task.WhenAny(tasksList.ToArray());
                
                runningTasks = GetRunningTasksCount(tasksList);
            }
        }

        await Task.WhenAll(tasksList.ToArray());
        
        var biggestScore = (long)0;

        foreach (var task in tasksList)
        {
            answersList.Add(await task);
        }
        
        _logger.Information("answersList.Count: {Count}", answersList.Count);
        
        foreach (var answer in answersList)
        {
            if (biggestScore < answer)
                biggestScore = answer;
        }
        
        LogStopwatchFinalTimes();
        
        _logger.Information("Final biggest score: {BiggestScore}", biggestScore);
    }

    private static int GetRunningTasksCount(ConcurrentBag<Task<long>> tasksList)
    {
        var runningTasks = 0;

        foreach (var task in tasksList)
        {
            if (task.IsCompleted) continue;

            runningTasks++;
        }

        return runningTasks;
    }

    private void LogStopwatchFinalTimes()
    {
        _elapsedTotal.Stop();

        var elapsedString = _elapsedTotal.Elapsed.ToString(@"hh\:mm\:ss\.fff");
        
        _logger.Information("Threads: {MaxThreadCount}", _maxThreads);
        _logger.Information("Final total time elapsed hh:mm:ss.fff: {TimeString}", elapsedString);
    }

    [SupportedOSPlatform("windows")]
    private long RunStartPoint(string textToRender, int dataWidth, int dataHeight, StartPoint startPoint)
    {
        List<Beam> allBeams = new List<Beam>();
        long biggestScore;
            
        var renderedBitmap = _textRenderer.GetBlankBoardBitmap(textToRender);
        BoardImage = AvaloniaImageHelper.ConvertToAvaloniaBitmap(renderedBitmap);

        WindowWidth = (int)BoardImage.Size.Width;
        WindowHeight = (int)BoardImage.Size.Height;

        allBeams.Clear();

        var answerSpaces = new bool[dataWidth, dataHeight];
        var squareBeamVisitCountSpaces = new int[dataWidth, dataHeight];

        allBeams.Add(
            new Beam(startPoint.Point.X, startPoint.Point.Y, startPoint.Direction, textToRender.Split(Environment.NewLine), allBeams, squareBeamVisitCountSpaces, null, _logger));

        //await Task.Delay(3000);

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

            // renderedBitmap = _beamRenderer.RenderAllBeams(renderedBitmap, allBeams);

            // renderedBitmap = _textRenderer.RenderTextOnImage(textToRender, renderedBitmap);

            // BoardImage = AvaloniaImageHelper.ConvertToAvaloniaBitmap(renderedBitmap);

            RemoveUnmovingLasersFrom(allBeams);

            // if (steps++ % 100 == 0)
            //     _logger.Information("Step count: {Steps}, allBeams.Count: {BeamsCount}", steps, allBeams.Count);

            // await Task.Delay(1);
        }

        var thisRoundAnswer = TallyEnergizedSquares(squareBeamVisitCountSpaces, dataWidth, dataHeight);

        // _logger.Information("From inside method thisRoundAnswer: {ThisRound}", thisRoundAnswer);
        
        return thisRoundAnswer;
    }

    private static void GetAllStartPoints(int dataWidth, List<StartPoint> startPoints, int dataHeight)
    {
        for (var x = 0; x < dataWidth; x++)
        {
            startPoints.Add(
                new StartPoint(
                    new Point(x, -1),
                    BeamDirections.Down));
            
            startPoints.Add(
                new StartPoint(
                    new Point(x, dataHeight + 1),
                    BeamDirections.Up));
        }

        for (var y = 0; y < dataHeight; y++)
        {
            startPoints.Add(
                new StartPoint(
                    new Point(-1, y),
                    BeamDirections.Right));
            
            startPoints.Add(
                new StartPoint(
                    new Point(dataWidth + 1, y),
                    BeamDirections.Left));
        }
    }

    private long TallyEnergizedSquares(int[,] squareBeamVisitCountSpaces, int dataWidth, int dataHeight)
    {
        var visitSpacesAnswer = 0;
        
        for (var x = 0; x < dataWidth; x++)
        {
            for (var y = 0; y < dataHeight; y++)
            {
                if (squareBeamVisitCountSpaces[x, y] > 0) visitSpacesAnswer++;
            }
        }
        
        _logger.Debug("Visit spaces count: {Count}", visitSpacesAnswer);
        
        return visitSpacesAnswer;
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
