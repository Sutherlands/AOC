using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2023
{
  public static class Day7
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay7.txt").ToList();

      var hands = lines.Select(ParseHand).ToList();
      hands.Sort();

      foreach (var hand in hands)
      {
        Console.WriteLine($"{hand.Cards} {hand.Bid}");
      }

      var total = hands.Select((hand, index) => hand.Bid * (index + 1)).Sum();
      Console.WriteLine(total);
    }

    private static Hand ParseHand(string line)
    {
      var parts = line.Split(' ');
      return new Hand { Cards = parts[0], Bid = int.Parse(parts[1]) };
    }

    public class Hand : IComparable<Hand>
    {
      public string Cards { get; set; }
      public int Bid { get; set; }

      public HandStrength GetStrength()
      {
        List<char> CardTypes = new List<char> { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };

        var dictionary = CardTypes.ToDictionary(c => c, c => Cards.Count(cc => cc == c));
        if (dictionary.Values.Any(v => v == 5))
        {
          return HandStrength.FiveOfAKind;
        }

        if (dictionary.Values.Any(v => v == 4))
        {
          return HandStrength.FourOfAKind;
        }

        if (dictionary.Values.Any(v => v == 3))
        {
          if (dictionary.Values.Any(v => v == 2))
          {
            return HandStrength.FullHouse;
          }
          else
          {
            return HandStrength.ThreeOfAKind;
          }
        }

        var pairs = dictionary.Values.Count(v => v == 2);
        switch (pairs)
        {
          case 2:
            return HandStrength.TwoPair;
          case 1:
            return HandStrength.OnePair;
          default:
            return HandStrength.HighCard;
        }
      }
      public HandStrength GetPart2Strength()
      {
        List<char> CardTypes = new List<char> { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };

        var dictionary = CardTypes.ToDictionary(c => c, c => Cards.Count(cc => cc == c));
        var jokers = Cards.Count(c => c == 'J');

        var maxValue = dictionary.Values.Max();
        var maxKey = dictionary.First(kvp => kvp.Value == maxValue).Key;
        dictionary[maxKey] = dictionary[maxKey] + jokers;

        if (dictionary.Values.Any(v => v == 5))
        {
          return HandStrength.FiveOfAKind;
        }

        if (dictionary.Values.Any(v => v == 4))
        {
          return HandStrength.FourOfAKind;
        }

        if (dictionary.Values.Any(v => v == 3))
        {
          if (dictionary.Values.Any(v => v == 2))
          {
            return HandStrength.FullHouse;
          }
          else
          {
            return HandStrength.ThreeOfAKind;
          }
        }

        var pairs = dictionary.Values.Count(v => v == 2);
        switch (pairs)
        {
          case 2:
            return HandStrength.TwoPair;
          case 1:
            return HandStrength.OnePair;
          default:
            return HandStrength.HighCard;
        }
      }

      public int CompareTo(Hand other)
      {
        var comparer = new CardComparer();
        var strengthComparison = GetStrength().CompareTo(other.GetStrength());
        if (strengthComparison != 0)
        {
          return strengthComparison;
        }

        for (var index = 0; index < 5; ++index)
        {
          var cardComparison = comparer.Compare(Cards[index], other.Cards[index]);
          if (cardComparison != 0)
          {
            return cardComparison;
          }
        }

        return 0;
      }
    }

    public class Part2HandComparer : IComparer<Hand>
    {
      public int Compare([AllowNull] Hand x, [AllowNull] Hand y)
      {
        var comparer = new Part2Comparer();
        var strengthComparison = x.GetPart2Strength().CompareTo(y.GetPart2Strength());
        if (strengthComparison != 0)
        {
          return strengthComparison;
        }

        for (var index = 0; index < 5; ++index)
        {
          var cardComparison = comparer.Compare(x.Cards[index], y.Cards[index]);
          if (cardComparison != 0)
          {
            return cardComparison;
          }
        }

        return 0;
      }
    }

    public class CardComparer : IComparer<char>
    {
      private readonly List<char> CardOrder = new List<char> { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
      public int Compare([AllowNull] char x, [AllowNull] char y)
      {
        return CardOrder.IndexOf(x).CompareTo(CardOrder.IndexOf(y));
      }
    }

    public class Part2Comparer : IComparer<char>
    {
      private readonly List<char> CardOrder = new List<char> { 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };
      public int Compare([AllowNull] char x, [AllowNull] char y)
      {
        return CardOrder.IndexOf(x).CompareTo(CardOrder.IndexOf(y));
      }
    }

    public enum HandStrength
    {
      HighCard,
      OnePair,
      TwoPair,
      ThreeOfAKind,
      FullHouse,
      FourOfAKind,
      FiveOfAKind
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay7.txt").ToList();

      var hands = lines.Select(ParseHand).ToList();
      hands.Sort(new Part2HandComparer());

      foreach (var hand in hands)
      {
        Console.WriteLine($"{hand.Cards} {hand.Bid} { hand.GetPart2Strength()}");
      }

      var total = hands.Select((hand, index) => hand.Bid * (index + 1)).Sum();
      Console.WriteLine(total);

    }

  }
}
