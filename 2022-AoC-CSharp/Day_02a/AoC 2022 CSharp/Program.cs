namespace AoC_2022_CSharp;

internal static class Program
{
    
    // "The first column is what your opponent is going to play:
    // A for Rock,
    // B for Paper,
    // C for Scissors

    // The second column, you reason, must be what you should play in response:
    // X for Rock,
    // Y for Paper,
    // Z for Scissors
    
    // The score for a single round is the score for the shape you selected:
    // 1 for Rock,
    // 2 for Paper,
    // 3 for Scissors
    //
    // plus the score for the outcome of the round:
    // 0 if you lost,
    // 3 if the round was a draw,
    // 6 if you won).
    
    public static char EnemyRock => 'A';
    public static char EnemyPaper => 'B';
    public static char EnemyScissors => 'C';
    
    public static char MeRock => 'X';
    public static char MePaper => 'Y';
    public static char MeScissors => 'Z';
    
    public static void Main()
    {
        var myScore = 0;

        foreach (var roundData in RawData.ActualData.Split(Environment.NewLine))
        {
            var roundResult = GetRoundResult(roundData);

            myScore += CalculateScore(roundData);
            
            Console.WriteLine($"Round input: {roundData}, with result: {roundResult}, and score added is: {CalculateScore(roundData)}");
            Console.WriteLine($"Total score now: {myScore}");
        }
    }

    private static int CalculateScore(string rawData)
    {
        var currentTotal = 0;

        if (rawData[2] == MeRock)
            currentTotal += 1;

        if (rawData[2] == MePaper)
            currentTotal += 2;
        
        if (rawData[2] == MeScissors)
            currentTotal += 3;

        if (GetRoundResult(rawData) == RoundStatus.Tied)
            currentTotal += 3;

        if (GetRoundResult(rawData) == RoundStatus.Won)
            currentTotal += 6;
        
        return currentTotal;
    }

    private static RoundStatus GetRoundResult(string rawData)
    {
        if (rawData[0] == EnemyRock)
        {   
            if (rawData[2] == MeRock) return RoundStatus.Tied;
            if (rawData[2] == MePaper) return RoundStatus.Won;
            if (rawData[2] == MeScissors) return RoundStatus.Lost;
        }
        
        if (rawData[0] == EnemyPaper)
        {   
            if (rawData[2] == MeRock) return RoundStatus.Lost;
            if (rawData[2] == MePaper) return RoundStatus.Tied;
            if (rawData[2] == MeScissors) return RoundStatus.Won;
        }
        
        if (rawData[0] == EnemyScissors)
        {   
            if (rawData[2] == MeRock) return RoundStatus.Won;
            if (rawData[2] == MePaper) return RoundStatus.Lost;
            if (rawData[2] == MeScissors) return RoundStatus.Tied;
        }

        throw new Exception();
    }

    private enum RoundStatus
    {
        Uninitialized,
        Won,
        Lost,
        Tied
    }
}