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

        var validGamesIdCounter = 0;
        
        foreach (var line in rawLines)
        {
            var redCubesLimit = 12;
            var greenCubesLimit = 13;
            var blueCubesLimit = 14;
            
            var gameId = line.Split(' ')[1].Replace(":", "").Replace("Game ", "");

            var cubeSetsRaw = line.Split(';');
            
            Logger.Debug("Raw line: {RawLine} | Game ID: '{GameId}' | Cube sets: {CubeSets}", line, gameId, string.Join(" - ", cubeSetsRaw));

            var allSetsValid = true;
            
            foreach (var cubeSet in cubeSetsRaw)
            {
                var redCubesAmount = GetCubesAmountInSet(cubeSet, "red");
                var greenCubesAmount = GetCubesAmountInSet(cubeSet, "green");
                var blueCubesAmount = GetCubesAmountInSet(cubeSet, "blue");

                Logger.Debug("In set; R: {RedAmountInSet}, G: {GreenAmountInSet}, B: {BlueAmountInSet}", redCubesAmount, greenCubesAmount, blueCubesAmount);

                if (redCubesAmount > redCubesLimit ||
                    greenCubesAmount > greenCubesLimit ||
                    blueCubesAmount > blueCubesLimit)
                {
                    allSetsValid = false;
                }
            }

            if (!allSetsValid) continue;
            
            var gameIdInt = int.Parse(gameId);
            validGamesIdCounter += gameIdInt;
                    
            Logger.Information("All sets in game under limits! Adding: {GameId} to total of IDs: {ValidGamesCounter}", gameIdInt, validGamesIdCounter);
        }
        
        Logger.Information("Answer: {IdsTotal}", validGamesIdCounter);
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