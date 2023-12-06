namespace Day1;

internal static class Program
{
    public static void Main()
    {
        var totals = GetTotals(RawData.ActualData);

        Console.WriteLine("Totals list:");
        Console.WriteLine(string.Join(Environment.NewLine, totals));
        
        totals.Sort();

        Console.WriteLine("Sorted totals:");
        Console.WriteLine(string.Join(Environment.NewLine, totals));
    }

    private static List<int> GetTotals(string rawData)
    {
        var returnTotals = new List<int>();

        var currentTotal = 0;
        
        foreach (var line in rawData.Split(Environment.NewLine))
        {
            Console.WriteLine($"Parsing line: {line}");
            Console.WriteLine($"Running total: {currentTotal}");
            Console.WriteLine();
            
            if (string.IsNullOrWhiteSpace(line))
            {
                returnTotals.Add(currentTotal);
                
                currentTotal = 0;
                
                continue;
            }
            
            // Otherwise, if not a blank line:
            currentTotal += int.Parse(line);      
        }

        return returnTotals;
    }
}