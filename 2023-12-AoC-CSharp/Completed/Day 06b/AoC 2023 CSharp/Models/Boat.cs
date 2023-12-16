using Serilog;
using Serilog.Core;

namespace AoC_2023_CSharp.Models;

public class Boat
{
    private readonly ILogger _logger;

    public Boat(ILogger logger, long buttonPressTime)
    {
        _logger = logger;
        ButtonPressTime = buttonPressTime;
        Speed = ButtonPressTime;
    }
    
    public long Speed { get; } = 0;

    public long Distance { get; private set; } = 0;
    
    public long ButtonPressTime { get; } = 0;

    public void RunRace(long raceTimeMs)
    {
        _logger.Debug("Button press time: {ButtonPressTime}", ButtonPressTime);
        _logger.Debug("Boat speed is: {BoatSpeed} mm/ms", Speed);

        var remainingRaceTime = raceTimeMs;
        var elapsedRaceTime = 0;
        
        while (remainingRaceTime-- > 0)
        {
            elapsedRaceTime++;
            
            if (elapsedRaceTime <= ButtonPressTime)
            {
                _logger.Verbose("Button being held at : {ElapsedTime} ms", elapsedRaceTime);
                
                continue;
            }
            
            Distance += Speed;
            
            _logger.Verbose("Elapsed race time: {ElapsedTime}", elapsedRaceTime);
            _logger.Verbose("Boat distance now: {BoatDistance}", Distance);
        }
        
        _logger.Debug("For race of time: {TotalRaceTime} ms, boat with button press {ButtonPressMs} travelled {Distance}", raceTimeMs, ButtonPressTime, Distance);
    }
}