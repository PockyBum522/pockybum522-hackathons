using Serilog;

namespace AoC_2022_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.BuildLogger(); 
    
    public static void Main()
    {
        var rawLines = RawData.ActualData
            .Split(Environment.NewLine);

        var edgeTreesCount = GetCountOfEdgeTrees(rawLines);
        
        var scoreList = GetNonEdgeVisibleTreesCount(rawLines);
        
        scoreList.Sort();

        foreach (var score in scoreList)
        {
            Logger.Information("Score: {Score}", score);
        }
        
        Logger.Debug("Number of edge (all automatically visible) trees: {NumberEdgeTrees}", edgeTreesCount);
        Logger.Debug("Width of data: {ColumnsInData} | Height of data: {RowsInData}", rawLines[0].Length, rawLines.Length);
        Logger.Debug("Number of non visible (but not on the edge) trees: {NumberInnerTrees}", scoreList);
        //Logger.Debug("Answer: {NumberTrees}", answerTotal);
    }

    private static List<int> GetNonEdgeVisibleTreesCount(string[] rawLines)
    {
        var scoreList = new List<int>();
        
        var y = 0;
        foreach (var row in rawLines)
        {
            y++;
                
            if (y > rawLines.Length - 2) continue;  // Skip last row cause we already got those
            
            // Start at 1 to miss the left edge, end before the right edge
            // NOTE WE MAY NOT WANT TO DO THIS FOR PART B BUT IM STARTING HERE
            for (int x = 1; x < row.Length - 1; x++)
            {
                scoreList.Add(
                    CalculateSenicScore(x, y, rawLines));
                
                Logger.Debug("x is: {XVal} | y is: {YVal} | Tree here is: {TreeVal}", x, y, rawLines[y][x]);
            }
        }

        return scoreList;
    }

    private static int CalculateSenicScore(int xPosition, int yPosition, string[] rawLines)
    {
        var scenicScoreNorth = GetNumberOfTreesVisibleTo(Direction.North, xPosition, yPosition, rawLines);
        var scenicScoreEast = GetNumberOfTreesVisibleTo(Direction.East, xPosition, yPosition, rawLines);
        var scenicScoreSouth = GetNumberOfTreesVisibleTo(Direction.South, xPosition, yPosition, rawLines);
        var scenicScoreWest = GetNumberOfTreesVisibleTo(Direction.West, xPosition, yPosition, rawLines);

        var totalScore = scenicScoreNorth * scenicScoreEast * scenicScoreSouth * scenicScoreWest;
        
        Logger.Debug("Scores: N: {NorthScore}, E: {EastScore}, S: {SouthScore}, W: {WastScore}", scenicScoreNorth, scenicScoreEast, scenicScoreSouth, scenicScoreWest);
        Logger.Debug("Score total: {TotalScore}", totalScore);

        return totalScore;
    }

    private static int GetNumberOfTreesVisibleTo(Direction dirToCheck, int xPosition, int yPosition, string[] rawLines)
    {
        var checkedTreeValue = int.Parse(rawLines[yPosition][xPosition].ToString());

        Logger.Debug("Checking tree at: X{XPos}, Y{YPos}, val: {TreeValue}", xPosition, yPosition, checkedTreeValue);

        var scoreCount = 0;
        
        switch (dirToCheck)
        {
            case Direction.North:
                for (var y = yPosition - 1; y >= 0; y--)
                {
                    Logger.Debug("Possible blocking tree to north at X{XPos}, Y{YPos}, val: {TreeValue}",xPosition, y, rawLines[y][xPosition]);
                    scoreCount++;
                    if (int.Parse(rawLines[y][xPosition].ToString()) >= checkedTreeValue) return scoreCount;
                }
                break;
            
            case Direction.East:
                for (var x = xPosition + 1; x < rawLines[0].Length; x++)
                {
                    Logger.Debug("Possible blocking tree to east at X{XPos}, Y{YPos}, val: {TreeValue}",x, yPosition, rawLines[yPosition][x]);
                    scoreCount++;
                    if (int.Parse(rawLines[yPosition][x].ToString()) >= checkedTreeValue) return scoreCount;
                }
                break;
            
            case Direction.South:
                for (var y = yPosition + 1; y < rawLines.Length; y++)
                {
                    Logger.Debug("Possible blocking tree to south at X{XPos}, Y{YPos}, val: {TreeValue}",xPosition, y, rawLines[y][xPosition]);
                    scoreCount++;
                    if (int.Parse(rawLines[y][xPosition].ToString()) >= checkedTreeValue) return scoreCount;
                }
                break;
            
            case Direction.West:
                for (var x = xPosition - 1; x >= 0; x--)
                {
                    Logger.Debug("Possible blocking tree to west at X{XPos}, Y{YPos}, val: {TreeValue}",x, yPosition, rawLines[yPosition][x]);
                    scoreCount++;
                    if (int.Parse(rawLines[yPosition][x].ToString()) >= checkedTreeValue) return scoreCount;
                }
                break;
        }

        return scoreCount;
    }

    private static int GetCountOfEdgeTrees(string[] rawLines)
    {
        return rawLines.Length * 2 +
               rawLines[0].Length * 2 -
               4; // So the corner trees don't get counted twice in the above two lines
    }

    public enum Direction
    {
        Uninitialized,
        North,
        East, 
        South,
        West
    }
}