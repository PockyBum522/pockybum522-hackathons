using System.Collections.Generic;
using System.Drawing;
using Aoc2023CSharp.Logic;
using DynamicData;

namespace Aoc2023CSharp.Models;

public class Beam
{
    private readonly List<Beam> _allBeams;
    private readonly bool[,] _answerSpaces;
    private readonly int[,] _beamsVisitedCountSpaces;

    private readonly ILogger? _logger;
    private readonly string[] _rawDataLines;

    public static ulong AllBeamsCount = 0;
    
    public Beam(
        int startX, 
        int startY, 
        BeamDirections directionAtStart, 
        string[] rawDataLines, 
        List<Beam> allBeams, 
        bool[,] answerSpaces, 
        int[,] beamsVisitedCountSpaces, 
        List<Point>? mirrorBounceSpaces,
        ILogger? logger = null)
    {
        _allBeams = allBeams;
        _answerSpaces = answerSpaces;
        _beamsVisitedCountSpaces = beamsVisitedCountSpaces;
        _logger = logger;
        _rawDataLines = rawDataLines;
        
        CurrentHeadX = startX;
        CurrentHeadY = startY;

        RenderFrom = RenderTo;
        
        CurrentDirection = directionAtStart;

        _hasTakenFirstStep = false;

        AllBeamsCount++;

        ThisBeamIndex = AllBeamsCount;

        MirrorBounceSpaces = new List<Point>();
        
        foreach (var bouncePoint in mirrorBounceSpaces ?? new List<Point>())
        {
            MirrorBounceSpaces.Add(bouncePoint);
        }
    }
    
    public int CurrentHeadX { get; private set; }
    public int CurrentHeadY { get; private set; }
    
    public PointF RenderFrom { get; private set; }
    public PointF RenderTo => new PointF(
        (float)(CurrentHeadX * BeamRenderer.HorizontalBoardSpacing + BeamRenderer.XStartOffset),
        (float)((CurrentHeadY * BeamRenderer.VerticalBoardSpacing) + BeamRenderer.YStartOffset));
    
    public BeamDirections CurrentDirection { get; }
    
    public bool IsStopped { get; private set; }

    private List<Point> MirrorBounceSpaces { get; set; } 

    public ulong ThisBeamIndex { get; private set;  }
    
    private int _margin = 3;
    
    private bool _hasTakenFirstStep;

    public bool TravelOneStep()
    {
        if (CurrentPositionIsWithinBounds())
        {
            _beamsVisitedCountSpaces[CurrentHeadX, CurrentHeadY]++;
        
            if (_beamsVisitedCountSpaces[CurrentHeadX, CurrentHeadY] > 100)
            {
                IsStopped = true;

                return true;    
            }
        }
        
        // if (MirrorBounceSpaces.Count > 200)
        //     _logger?.Information("Mirrorbounce count {Count}", MirrorBounceSpaces.Count);
        //
        // if (MirrorAtCurrentLocation())
        // {
        //     MirrorBounceSpaces.Add(
        //         new Point(CurrentHeadX, CurrentHeadY));
        //
        //     if (BeamHasHitMirrorXTimesBefore(5))
        //     {
        //         IsStopped = true;
        //
        //         return true;
        //     }
        // }

        if (_hasTakenFirstStep)
        {
            if (IsStopped) return false;
            
            if (MirrorOrSplitterAtCurrentLocation())
            {
                SpawnNewBeam();
            
                IsStopped = true;
            
                _logger?.Debug("Beam {Index}: IsStopped: {IsStopped}", ThisBeamIndex, IsStopped);

                return true;
            }
        }
        
        _hasTakenFirstStep = true;

        if (CurrentPositionIsWithinBounds())
        {
            var characterAtCurrentPosition = _rawDataLines[CurrentHeadY][CurrentHeadX];
            
            // Log square as energized
            _answerSpaces[CurrentHeadX, CurrentHeadY] = true;
            
            _logger?.Debug("Beam {Index} current char: {CurrentChar}", ThisBeamIndex, characterAtCurrentPosition);
        }
        else
        {
            _logger?.Debug("No current char, beam {Index} not within bounds", ThisBeamIndex);
        }
        
        var newY = CurrentHeadY;
        var newX = CurrentHeadX;

        RenderFrom = RenderTo;
        
        switch (CurrentDirection)
        {
            case BeamDirections.Uninitialized:
                if (!IsStopped)
                    throw new Exception("Beam direction not initialized, cannot step");
                break;
                
            case BeamDirections.Up:
                
                newY = CurrentHeadY - 1;

                if (newY < _margin * -1)
                    IsStopped = true;
                break;
            
            case BeamDirections.Right:

                newX = CurrentHeadX + 1;
                
                if (newX > _margin + _rawDataLines[0].Length)
                    IsStopped = true;
                
                break;
            
            case BeamDirections.Down:
                
                newY = CurrentHeadY + 1;

                if (newY > _margin + _rawDataLines.Length)
                    IsStopped = true;
                break;
            
            case BeamDirections.Left:
            
                newX = CurrentHeadX - 1;
                
                if (newX < _margin * -1)
                    IsStopped = true;
                
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }

        CurrentHeadX = newX;
        CurrentHeadY = newY;

        return false;
    }

    private bool BeamHasHitMirrorXTimesBefore(int timesToConsiderLooping)
    {
        var numberOfTimesThisMirrorSeen = 0;
        
        foreach (var mirrorBouncePosition in MirrorBounceSpaces)
        {
            if (mirrorBouncePosition.X == CurrentHeadX &&
                mirrorBouncePosition.Y == CurrentHeadY)
            {
                numberOfTimesThisMirrorSeen++;
            }
        }
        
        //_logger?.Information("Beam {Index} has hit this mirror {NumberOfTimes} times before at x: {MirrorX} y: {MirrorY}", ThisBeamIndex, numberOfTimesThisMirrorSeen, CurrentHeadX, CurrentHeadY);

        return numberOfTimesThisMirrorSeen > timesToConsiderLooping;
    }

    private bool CurrentPositionIsWithinBounds()
    {
        return
            CurrentHeadX >= 0 &&
            CurrentHeadX < _rawDataLines[0].Length &&
            CurrentHeadY >= 0 &&
            CurrentHeadY < _rawDataLines.Length;
    }

    private void SpawnNewBeam()
    {
        var characterAtCurrentPosition = _rawDataLines[CurrentHeadY][CurrentHeadX];
        
        _logger?.Debug("Spawning new beam because char at oldbeam position was: {CurrentChar}", characterAtCurrentPosition);

        if (characterAtCurrentPosition == '/')
        {
            if (CurrentDirection == BeamDirections.Up)
                _allBeams.Add(new Beam(CurrentHeadX, CurrentHeadY, BeamDirections.Right, _rawDataLines, _allBeams, _answerSpaces, _beamsVisitedCountSpaces, MirrorBounceSpaces, _logger));
            
            if (CurrentDirection == BeamDirections.Right)
                _allBeams.Add(new Beam(CurrentHeadX, CurrentHeadY, BeamDirections.Up, _rawDataLines, _allBeams, _answerSpaces, _beamsVisitedCountSpaces, MirrorBounceSpaces, _logger));
            
            if (CurrentDirection == BeamDirections.Down)
                _allBeams.Add(new Beam(CurrentHeadX, CurrentHeadY, BeamDirections.Left, _rawDataLines, _allBeams, _answerSpaces, _beamsVisitedCountSpaces, MirrorBounceSpaces, _logger));
            
            if (CurrentDirection == BeamDirections.Left)
                _allBeams.Add(new Beam(CurrentHeadX, CurrentHeadY, BeamDirections.Down, _rawDataLines, _allBeams, _answerSpaces, _beamsVisitedCountSpaces, MirrorBounceSpaces, _logger));
        }

        if (characterAtCurrentPosition == '\\')
        {
            if (CurrentDirection == BeamDirections.Up)
                _allBeams.Add(new Beam(CurrentHeadX, CurrentHeadY, BeamDirections.Left, _rawDataLines, _allBeams, _answerSpaces, _beamsVisitedCountSpaces, MirrorBounceSpaces, _logger));
            
            if (CurrentDirection == BeamDirections.Right)
                _allBeams.Add(new Beam(CurrentHeadX, CurrentHeadY, BeamDirections.Down, _rawDataLines, _allBeams, _answerSpaces, _beamsVisitedCountSpaces, MirrorBounceSpaces, _logger));
            
            if (CurrentDirection == BeamDirections.Down)
                _allBeams.Add(new Beam(CurrentHeadX, CurrentHeadY, BeamDirections.Right, _rawDataLines, _allBeams, _answerSpaces, _beamsVisitedCountSpaces, MirrorBounceSpaces, _logger));
            
            if (CurrentDirection == BeamDirections.Left)
                _allBeams.Add(new Beam(CurrentHeadX, CurrentHeadY, BeamDirections.Up, _rawDataLines, _allBeams, _answerSpaces, _beamsVisitedCountSpaces, MirrorBounceSpaces, _logger));
        }

        if (characterAtCurrentPosition == '-')
        {
            if (CurrentDirection == BeamDirections.Up ||
                CurrentDirection == BeamDirections.Down)
            {
                _allBeams.Add(new Beam(CurrentHeadX, CurrentHeadY, BeamDirections.Left, _rawDataLines, _allBeams, _answerSpaces, _beamsVisitedCountSpaces, MirrorBounceSpaces, _logger));
                _allBeams.Add(new Beam(CurrentHeadX, CurrentHeadY, BeamDirections.Right, _rawDataLines, _allBeams, _answerSpaces, _beamsVisitedCountSpaces, MirrorBounceSpaces, _logger));
            }
                
            // Just keep goin' (Albeit with a new beam, but it looks the same.
            if (CurrentDirection == BeamDirections.Right)
                _allBeams.Add(new Beam(CurrentHeadX, CurrentHeadY, BeamDirections.Right, _rawDataLines, _allBeams, _answerSpaces, _beamsVisitedCountSpaces, MirrorBounceSpaces, _logger));
            
            if (CurrentDirection == BeamDirections.Left)
                _allBeams.Add(new Beam(CurrentHeadX, CurrentHeadY, BeamDirections.Left, _rawDataLines, _allBeams, _answerSpaces, _beamsVisitedCountSpaces, MirrorBounceSpaces, _logger));
        }

        if (characterAtCurrentPosition == '|')
        {
            if (CurrentDirection == BeamDirections.Right ||
                CurrentDirection == BeamDirections.Left)
            {
                _allBeams.Add(new Beam(CurrentHeadX, CurrentHeadY, BeamDirections.Up, _rawDataLines, _allBeams, _answerSpaces, _beamsVisitedCountSpaces, MirrorBounceSpaces, _logger));
                _allBeams.Add(new Beam(CurrentHeadX, CurrentHeadY, BeamDirections.Down, _rawDataLines, _allBeams, _answerSpaces, _beamsVisitedCountSpaces, MirrorBounceSpaces, _logger));
            }
                
            // Just keep goin' (Albeit with a new beam, but it looks the same.
            if (CurrentDirection == BeamDirections.Up)
                _allBeams.Add(new Beam(CurrentHeadX, CurrentHeadY, BeamDirections.Up, _rawDataLines, _allBeams, _answerSpaces, _beamsVisitedCountSpaces, MirrorBounceSpaces, _logger));
            
            if (CurrentDirection == BeamDirections.Down)
                _allBeams.Add(new Beam(CurrentHeadX, CurrentHeadY, BeamDirections.Down, _rawDataLines, _allBeams, _answerSpaces, _beamsVisitedCountSpaces, MirrorBounceSpaces, _logger));
        }
    }

    private bool MirrorOrSplitterAtCurrentLocation()
    {
        if (!CurrentPositionIsWithinBounds()) return false;
        
        var characterAtCurrentPosition = _rawDataLines[CurrentHeadY][CurrentHeadX];

        if (characterAtCurrentPosition == '/' ||
            characterAtCurrentPosition == '\\' ||
            characterAtCurrentPosition == '-' ||
            characterAtCurrentPosition == '|')
        {
            return true;
        }

        return false;
    }    
    
    private bool MirrorAtCurrentLocation()
    {
        if (!CurrentPositionIsWithinBounds()) return false;
        
        var characterAtCurrentPosition = _rawDataLines[CurrentHeadY][CurrentHeadX];

        if (characterAtCurrentPosition == '/')
        {
            return true;
        }

        return false;
    }
}

public enum BeamDirections
{
    Uninitialized,
    Up,
    Right,
    Down,
    Left
}