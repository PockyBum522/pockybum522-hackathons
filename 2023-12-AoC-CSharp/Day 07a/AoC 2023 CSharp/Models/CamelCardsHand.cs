using AoC_2023_CSharp.Utilities;

namespace AoC_2023_CSharp.Models;

public class CamelCardsHand
{
    public CamelCardsHand(string inputLine)
    {
        var elements = inputLine.SplitGeneric<string>(" ");

        Bid = int.Parse(elements[1]);

        foreach (var cardValueChar in elements[0])
        {
            Cards.Add(
                new CamelCard(cardValueChar));
        }
    }
    
    public int Bid { get; }

    public List<CamelCard> Cards = new();

    public CamelCardHandStrengthEnum HandStrength => CalculateHandStrength();

    private CamelCardHandStrengthEnum CalculateHandStrength()
    {
        var uniqueCardsInHand = new List<CamelCard>();

        // Make list of unique cards 
        foreach (var card in Cards)
        {
            if (Cards.All(camelCard => camelCard.Value == card.Value))
                uniqueCardsInHand.Add(card);
        }

        // for each card in list, make note of how many of that card there is
        foreach (var card in uniqueCardsInHand)
        {
            var cardsOfThisValueCount = Cards.Count(camelCard => camelCard.Value == card.Value);

            card.NumberInHand = cardsOfThisValueCount;
        }

        uniqueCardsInHand.Sort(c => c.NumberInHand);
    }
}

public class CamelCard
{
    public CamelCard(char value)
    {
        Value = value;
    }
    
    public char Value { get; }
    
    public int NumberInHand { get; set; }
}

public enum CamelCardHandStrengthEnum
{
    Uninitialized,
    HighCard,
    OnePair,
    TwoPair,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind
    
    // Five of a kind, where all five cards have the same label: AAAAA
    // Four of a kind, where four cards have the same label and one card has a different label: AA8AA
    // Full house, where three cards have the same label, and the remaining two cards share a different label: 23332
    // Three of a kind, where three cards have the same label, and the remaining two cards are each different from any other card in the hand: TTT98
    // Two pair, where two cards share one label, two other cards share a second label, and the remaining card has a third label: 23432
    // One pair, where two cards share one label, and the other three cards have a different label from the pair and each other: A23A4
    // High card, where all cards' labels are distinct: 23456
}