using Serilog;

namespace AoC_2022_CSharp;

internal static class Program
{
    public static ILogger Logger = LoggerSetup.BuildLogger(); 
    
    public static void Main()
    {
        var moveLines = RawData.ActualDataMoves
            .Split(Environment.NewLine);

        // Number of columns we need to store, adjust per stack data. Counted horizontally. Should be the same number
        // at the bottom of the raw string from AoC on bottom rightmost.  
        const int stackColumnsCount = 9;
        
        var stacks = ConvertStacksString(RawData.ActualDataStacks, stackColumnsCount);

        foreach (var line in moveLines)
        {
            Logger.Debug("Instruction: {Line}", line);

            ExecuteMove(stacks, line);
        }
        
        // Output stacks final configuration
        for (var i = 0; i < stackColumnsCount; i++)
        {
            Logger.Debug("Stack representing column: {ColumnNumber}. Contents top to bottom:", i);
            
            foreach (var element in stacks[i] ?? throw new NullReferenceException("Stack in array was null"))
            {
                Logger.Debug("Contains: {Element}", element);
            }
        }
        
        // Output stacks tops only configuration
        var finalAnswer = "";
        for (var i = 0; i < stackColumnsCount; i++)
        {
            finalAnswer += stacks[i]!.Pop();
        }
        Logger.Debug("Top elements of all stacks: {FinalAnswer}", finalAnswer);
    }
    
    private static Stack<char>?[] ConvertStacksString(string stacksRaw, int stackColumnsCount)
    {
        var parsedStacks = new Stack<char>?[stackColumnsCount];

        var startTextColumn = 1;
        var rawStackStringHorizontalSpacing = 4;
        var stackColumnCounter = -1;
        
        //     [D]
        // [N] [C]
        // [Z] [M] [P]
        //  1   2   3

        var rawStackStringLines = stacksRaw.Split(Environment.NewLine);
        
        // Parse columns, left to right
        for (var i = startTextColumn; i < stackColumnsCount * rawStackStringHorizontalSpacing; i += rawStackStringHorizontalSpacing)
        {
            // Increment what actual stack in array we're putting things into as we parse
            stackColumnCounter++;
            
            // Start at the bottom and move up the column to the top
            for (var j = rawStackStringLines.Length - 1; j >= 0; j--)
            {
                var currentChar = rawStackStringLines[j][i];

                var charIsLetter = char.IsLetter(currentChar);

                parsedStacks[stackColumnCounter] ??= new Stack<char>();
                
                if (charIsLetter)
                    parsedStacks[stackColumnCounter]!.Push(currentChar);
                
                Logger.Debug("Parsing col: {ColNumber}, row: {RowNumber} - Char found: {CharAtPosition}", i, j, currentChar);
                Logger.Debug("Char is letter? {CharIsLetter}", charIsLetter);
            }
        }
        
        // Debug stacks to make sure we're looking good
        for (var i = 0; i < stackColumnsCount; i++)
        {
            Logger.Debug("Stack representing column: {ColumnNumber}. Contents top to bottom:", i);
            
            foreach (var element in parsedStacks[i] ?? throw new NullReferenceException("Stack in array was null"))
            {
                Logger.Debug("Contains: {Element}", element);
            }
        }

        return parsedStacks;
    }
    
    private static void ExecuteMove(Stack<char>?[] stacks, string line)
    {
        var splitInstruction = line.Split(' ');

        var moveNumber = int.Parse(splitInstruction[1]);
        
        // Working with zero-indexed array so we have to subtract 1 from the 1-indexed instructions
        var fromNumber = int.Parse(splitInstruction[3]) - 1;
        var toNumber = int.Parse(splitInstruction[5]) - 1;

        Logger.Debug("Raw instruction line: {RawLine} | Interpreted: move {Count} from {FromNum} to {ToNum}", line, moveNumber, fromNumber, toNumber);

        var cratesMoving = new List<char>();
        
        for (var i = 0; i < moveNumber; i++)
        {
            var poppedValue = stacks[fromNumber]!.Pop();
            
            cratesMoving.Add(poppedValue);
        }

        for (var i = cratesMoving.Count - 1; i >= 0; i--)
        {
            stacks[toNumber]!.Push(
                cratesMoving[i]);
        }
    }
}