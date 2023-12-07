using Serilog;

namespace AoC_2022_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.BuildLogger(); 
    
    public static void Main()
    {
        var rawLines = RawData.ActualData
            .Split(Environment.NewLine);
        
        // 12 red cubes, 13 green cubes, and 14 blue cubes

        var totalCumulativePower = 0;
        
        foreach (var line in rawLines)
        {
            var redCubesMinimum = 0;
            var greenCubesMinimum = 0;
            var blueCubesMinimum = 0;
            
            var gameId = line.Split(' ')[1].Replace(":", "").Replace("Game ", "");

            var cubeSetsRaw = line.Split(';');
            
            Logger.Debug("Raw line: {RawLine} | Game ID: '{GameId}' | Cube sets: {CubeSets}", line, gameId, string.Join(" - ", cubeSetsRaw));
            
            foreach (var cubeSet in cubeSetsRaw)
            {
                var redCubesAmount = GetCubesAmountInSet(cubeSet, "red");
                var greenCubesAmount = GetCubesAmountInSet(cubeSet, "green");
                var blueCubesAmount = GetCubesAmountInSet(cubeSet, "blue");
                
                // Set new minimum if we found more than the old minimum
                if (redCubesAmount > redCubesMinimum)
                    redCubesMinimum = redCubesAmount;

                if (greenCubesAmount > greenCubesMinimum)
                    greenCubesMinimum = greenCubesAmount;

                if (blueCubesAmount > blueCubesMinimum)
                    blueCubesMinimum = blueCubesAmount;
                
                Logger.Debug("In set; R:{RedAmountInSet}, G:{GreenAmountInSet}, B:{BlueAmountInSet}", redCubesAmount, greenCubesAmount, blueCubesAmount);
                Logger.Debug("Minimums are now: R:{RedMin}, G:{GreenMin}, B:{BlueMin}", redCubesMinimum, greenCubesMinimum, blueCubesMinimum);
            }

            var totalPower = redCubesMinimum * greenCubesMinimum * blueCubesMinimum;
            
            Logger.Debug("Minimum for all sets calculated. Calculating power: {RedMin} * {GreenMin} * {BlueMin} = {Total}", redCubesMinimum, greenCubesMinimum, blueCubesMinimum, totalPower);
            
            totalCumulativePower += totalPower;
        }
        
        Logger.Information("Answer: {IdsTotal}", totalCumulativePower);
    }

    private static int GetCubesAmountInSet(string cubeSet, string cubeColor)
    {
        Logger.Debug("Parsing set: {SetRaw} looking for {Color}", cubeSet, cubeColor);

        var splitOnSpaces = cubeSet.Split(" ");

        for (var i = 0; i < splitOnSpaces.Length; i++)
        {
            var section = splitOnSpaces[i].TrimEnd(',');
            
            //Logger.Debug("Checking section: '{CurrentSection}'", section);

            if (section != cubeColor) continue;
            
            // Otherwise, found it:
            var sectionAmount = int.Parse(splitOnSpaces[i - 1]);
                
            Logger.Debug("Found: '{CubeColor}' and amount: {CubeAmount}", cubeColor, sectionAmount);

            return sectionAmount;
        }
        
        return 0;  // If we can't match the color when it's not there, then it's always valid 
    }
}