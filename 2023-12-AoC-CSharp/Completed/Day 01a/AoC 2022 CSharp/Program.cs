namespace AoC_2022_CSharp;

internal static class Program
{
    public static void Main()
    {
        var dataLines = RawData.ActualData
            .Split(Environment.NewLine);

        var total = 0;
        
        foreach (var line in dataLines)
        {
            var firstNumber = GetNumbers(line).First();

            var secondNumber = GetNumbers(line).Last();

            var fullString = firstNumber.ToString() + secondNumber.ToString();

            Console.WriteLine($"For line: {line} | Got: {firstNumber} and {secondNumber} | {fullString}");
            
            total += int.Parse(fullString);
        }

        Console.WriteLine($"Total: {total}");
    }

    private static List<int> GetNumbers(string dataLine)
    {
        var returnList = new List<int>();
        
        foreach (var character in dataLine)
        {
            if (!char.IsDigit(character)) continue;

            returnList.Add(int.Parse(character.ToString()));
        }

        return returnList;
    }
}