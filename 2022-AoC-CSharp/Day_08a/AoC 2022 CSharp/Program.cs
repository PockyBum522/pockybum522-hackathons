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
        
        var innerVisibleTreesCount = GetNonEdgeVisibleTreesCount(rawLines);
        
        var answerTotal = edgeTreesCount + innerVisibleTreesCount;
        
        Logger.Information("Number of edge (all automatically visible) trees: {NumberEdgeTrees}", edgeTreesCount);
        Logger.Information("Width of data: {ColumnsInData} | Height of data: {RowsInData}", rawLines[0].Length, rawLines.Length);
        Logger.Information("Number of non visible (but not on the edge) trees: {NumberInnerTrees}", innerVisibleTreesCount);
        Logger.Information("Answer: {NumberTrees}", answerTotal);
    }

    private static int GetNonEdgeVisibleTreesCount(string[] rawLines)
    {
        var totalTreesVisible = 0;
        
        var y = 0;
        foreach (var row in rawLines)
        {
            y++;
                
            if (y > rawLines.Length - 2) continue;  // Skip last row cause we already got those
            
            // Start at 1 to miss the left edge, end before the right edge 
            for (int x = 1; x < row.Length - 1; x++)
            {
                if (TreeIsVisibleFromAnyDirection(x, y, rawLines))
                    totalTreesVisible++;
                
                Logger.Debug("Total trees visible now: {TotalTreesVisible}", totalTreesVisible);
                
                Logger.Debug("x is: {XVal} | y is: {YVal} | Tree here is: {TreeVal}", x, y, rawLines[y][x]);
            }
        }

        return totalTreesVisible;
    }

    private static bool TreeIsVisibleFromAnyDirection(int xPosition, int yPosition, string[] rawLines)
    {
        if (TreeIsVisibleFrom(Direction.North, xPosition, yPosition, rawLines)) return true;
        if (TreeIsVisibleFrom(Direction.East, xPosition, yPosition, rawLines)) return true;
        if (TreeIsVisibleFrom(Direction.South, xPosition, yPosition, rawLines)) return true;
        if (TreeIsVisibleFrom(Direction.West, xPosition, yPosition, rawLines)) return true;

        return false;
    }

    private static bool TreeIsVisibleFrom(Direction dirToCheck, int xPosition, int yPosition, string[] rawLines)
    {
        var checkedTreeValue = int.Parse(rawLines[yPosition][xPosition].ToString());

        Logger.Debug("Checking tree at: X{XPos}, Y{YPos}, val: {TreeValue}", xPosition, yPosition, checkedTreeValue);

        switch (dirToCheck)
        {
            case Direction.North:
                for (var y = yPosition - 1; y >= 0; y--)
                {
                    Logger.Debug("Possible blocking tree to north at X{XPos}, Y{YPos}, val: {TreeValue}",xPosition, y, rawLines[y][xPosition]);
                    if (int.Parse(rawLines[y][xPosition].ToString()) >= checkedTreeValue) return false;
                }
                break;
            
            case Direction.East:
                for (var x = xPosition + 1; x < rawLines[0].Length; x++)
                {
                    Logger.Debug("Possible blocking tree to east at X{XPos}, Y{YPos}, val: {TreeValue}",x, yPosition, rawLines[yPosition][x]);
                    if (int.Parse(rawLines[yPosition][x].ToString()) >= checkedTreeValue) return false;
                }
                break;
            
            case Direction.South:
                for (var y = yPosition + 1; y < rawLines.Length; y++)
                {
                    Logger.Debug("Possible blocking tree to south at X{XPos}, Y{YPos}, val: {TreeValue}",xPosition, y, rawLines[y][xPosition]);
                    if (int.Parse(rawLines[y][xPosition].ToString()) >= checkedTreeValue) return false;
                }
                break;
            
            case Direction.West:
                for (var x = xPosition - 1; x >= 0; x--)
                {
                    Logger.Debug("Possible blocking tree to west at X{XPos}, Y{YPos}, val: {TreeValue}",x, yPosition, rawLines[yPosition][x]);
                    if (int.Parse(rawLines[yPosition][x].ToString()) >= checkedTreeValue) return false;
                }
                break;
        }

        return true;
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