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

            var oneSectionFullyContained = OneSectionIsFullyContainedInOther(firstSectionNumbers, secondSectionNumbers);
            
            if (oneSectionFullyContained)
                total++; 
            
            Logger.Debug("Sections: {First}, {Second}", firstSection, secondSection);
            Logger.Debug("Fully contained? {FullyContained}", oneSectionFullyContained);
        }

        Logger.Information("Total: {Total}", total);
    }

    private static int[] GetNumbersFromSection(string section)
    {
        var firstNumberString = section.Split('-')[0];
        var secondNumberString = section.Split('-')[1];

        return new[] { int.Parse(firstNumberString), int.Parse(secondNumberString) };
    }
    
    private static bool OneSectionIsFullyContainedInOther(IReadOnlyList<int> firstSection, IReadOnlyList<int> secondSection)
    {
        if (firstSection[0] <= secondSection[0] &&
            firstSection[1] >= secondSection[1])
        {
            return true;
        }
        
        if (secondSection[0] <= firstSection[0] &&
            secondSection[1] >= firstSection[1])
        {
            return true;
        }

        return false;
    }
}