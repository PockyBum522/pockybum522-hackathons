using Serilog;
using Serilog.Core;

namespace AoC_2023_CSharp.Models;

public class Loop
{
    private readonly ILogger? _logger;

    public Loop(ulong startAt, ulong period, ILogger? loggerToUse = null)
    {
        _logger = loggerToUse;

        StartPosition = startAt;
        
        Period = period;
    }
    
    public ulong StartPosition { get; set; }

    public ulong Period { get; set; }

    public ulong CurrentValueAtMultiple(ulong multiple) => StartPosition + (Period * multiple); 
  
    public ulong GetClosestValueTo(ulong valueToNotExceed)
    {
        _logger?.Debug("Checking loop with start: {LoopStart} and period: {LoopPeriod} against value to not exceed: {MaxValue}", StartPosition, Period, valueToNotExceed);
        
        var startCheckAt = valueToNotExceed / (double)Period;

        _logger?.Debug("Starting at (raw): {CheckStartValue}", startCheckAt);
        
        startCheckAt = Math.Floor(startCheckAt);

        var startCheckAtRounded = (ulong)startCheckAt;
        
        _logger?.Debug("Starting at (double: {CheckStartValue}) (ulong: {RoundedValue})", startCheckAt, startCheckAtRounded);

        var stopCheckAt = startCheckAtRounded + 3;
        
        for (var i = startCheckAtRounded; i < stopCheckAt; i++)
        {
            var currentLoopValue = StartPosition + (i * Period);
            
            _logger?.Debug("Checking loop at: {LoopValue} and multiple: {CurrentMultiple} against value to not exceed: {MaxValue}", currentLoopValue, Period, valueToNotExceed);

            if (IsWithin5(currentLoopValue, valueToNotExceed))
                _logger?.Information("WAS WITHIN 5 - Checking loop at: {LoopValue} and multiple: {CurrentMultiple} against value to not exceed: {MaxValue}", currentLoopValue, Period, valueToNotExceed);
            
            if (IsWithin5(currentLoopValue, valueToNotExceed))
                return currentLoopValue;
            
            if (currentLoopValue > valueToNotExceed)
                return currentLoopValue - Period;
        }

        throw new Exception(
            "Value when checking loop against valueToNotExceed never exceeded value which should never happen");
    }
    
    private bool IsWithin5(ulong checkValue, ulong matchValue)
    {
        if (checkValue + 1000 > matchValue && 
            checkValue - 1000 < matchValue)
        {
            return true;
        }

        return false;
    }
}