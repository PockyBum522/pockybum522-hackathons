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
            var dumbPaddedLine = line + "zzzzzzz";
            
            Console.WriteLine($"Testing {dumbPaddedLine}");
            
            var firstNumber = GetNumbers(dumbPaddedLine).First();

            var secondNumber = GetNumbers(dumbPaddedLine).Last();

            var fullString = firstNumber.ToString() + secondNumber.ToString();

            Console.WriteLine($"For line: {dumbPaddedLine} | Got: {string.Join(", ", GetNumbers(dumbPaddedLine))} first and last: {fullString}");
            
            total += int.Parse(fullString);
        }

        Console.WriteLine($"Total: {total}");
    }

    private static List<int> GetNumbers(string dataLine)
    {
        var returnList = new List<int>();

        for (var i = 0; i < dataLine.Length; i++)
        {
            var character = dataLine[i];

            // Console.WriteLine($"Testing at position: {i}, char is: {character}");
            
            if (char.IsDigit(character))
            {
                // Console.WriteLine("Digit found, saving.");
                
                returnList.Add(int.Parse(character.ToString()));
                continue;
            }

            // Console.WriteLine("Wasn't digit, testing for text numbers");
            
            var textNumber = GetTextNumberStartingAt(i, dataLine);
            
            if (textNumber != -1)
            {
                // Console.WriteLine($"Found text number: {textNumber}, saving");
                
                returnList.Add(textNumber);
            }
        }

        return returnList;
    }

    private static int GetTextNumberStartingAt(int startPosition, string dataLine)
    {
        // Console.WriteLine($"Checking for text numbers at start of substring: {dataLine.Substring(startPosition)}");
        
        try
        {
            if (dataLine.Substring(startPosition, 3) == "one")
            {
                // Console.WriteLine("Matched 'one'");
                
                return 1;
            }

            if (dataLine.Substring(startPosition, 3) == "two")
            {
                // Console.WriteLine("Matched 'two'");

                return 2;
            }

            if (dataLine.Substring(startPosition, 5) == "three")
            {
                // Console.WriteLine("Matched 'three'");

                return 3;
            }

            if (dataLine.Substring(startPosition, 4) == "four")
            {                    
                // Console.WriteLine("Matched 'four'");

                return 4;
            }

            if (dataLine.Substring(startPosition, 4) == "five")
            {                    
                // Console.WriteLine("Matched 'five'");

                return 5;
            }

            if (dataLine.Substring(startPosition, 3) == "six")
            {                    
                // Console.WriteLine("Matched 'six'");

                return 6;
            }

            if (dataLine.Substring(startPosition, 5) == "seven")
            {
                // Console.WriteLine("Matched 'seven'");

                return 7;
            }

            if (dataLine.Substring(startPosition, 5) == "eight")
            {
                // Console.WriteLine("Matched 'eight'");

                return 8;
            }

            if (dataLine.Substring(startPosition, 4) == "nine")
            {
                // Console.WriteLine("Matched 'nine'");

                return 9;
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            return -1;
        }

        return -1;
    }
}