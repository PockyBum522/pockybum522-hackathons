using Serilog;

namespace AoC_2022_CSharp;

internal static class Program
{
    public static ILogger Logger = LoggerSetup.BuildLogger(); 
    
    public static void Main()
    {
        var dataLines = RawData.ActualData
            .Split(Environment.NewLine);

        var total = 0;

        for (var i = 0; i < dataLines.Length; i++)
        {
            // Split on comma to get the two sections
            var firstSection = dataLines[i].Split(',')[0]; 
            var secondSection = dataLines[i].Split(',')[1];

            var firstSectionNumbers = GetNumbersFromSection(firstSection);
            var secondSectionNumbers = GetNumbersFromSection(secondSection);

            var doSectionsOverlap = OneSectionOverlapsOther(firstSectionNumbers, secondSectionNumbers);
            
            if (doSectionsOverlap)
                total++; 
            
            Logger.Debug("Sections: {First}, {Second}", firstSection, secondSection);
            Logger.Debug("Overlap? {FullyContained}", doSectionsOverlap);
        }

        Logger.Information("Total: {Total}", total);
    }

    private static int[] GetNumbersFromSection(string section)
    {
        var firstNumberString = section.Split('-')[0];
        var secondNumberString = section.Split('-')[1];

        return new[] { int.Parse(firstNumberString), int.Parse(secondNumberString) };
    }
    
    private static bool OneSectionOverlapsOther(IReadOnlyList<int> firstSection, IReadOnlyList<int> secondSection)
    {
        var sectionOneRangeNumbers = new List<int>();
        var sectionTwoRangeNumbers = new List<int>();
        
        // Get section 1 range
        for (var secOneIndex = firstSection[0]; secOneIndex <= firstSection[1]; secOneIndex++)
        {
            sectionOneRangeNumbers.Add(secOneIndex);
        }
        
        // Get section 2 range
        for (var secTwoIndex = secondSection[0]; secTwoIndex <= secondSection[1]; secTwoIndex++)
        {
            sectionTwoRangeNumbers.Add(secTwoIndex);
        }

        // Check section 1 range against overlap in section 2 range
        foreach (var numberToCheck in sectionOneRangeNumbers)
        {
            if (sectionTwoRangeNumbers.Contains(numberToCheck))
                return true;
        }

        return false;
    }
}