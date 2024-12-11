using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2024
{
  public static class Day11
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay11.txt").ToList();
      var numbers = lines[0].Split(' ').Select(long.Parse);
      for (int x = 0; x < 25; ++x)
      {
        numbers = Blink(numbers);
      }

      Console.WriteLine(numbers.Count());
    }

    //2024 = 11*13*2*2*2
    private static IEnumerable<long> Blink(IEnumerable<long> numbers)
    {
      foreach (var number in numbers)
      {
        var ns = number.ToString();
        if (number == 0)
        {
          yield return 1;
        }
        else if (ns.Length % 2 == 0)
        {
          yield return long.Parse(string.Join("", ns.Take(ns.Length / 2)));
          yield return long.Parse(string.Join("", ns.Skip(ns.Length / 2)));
        }
        else
        {
          yield return number * 2024;
        }
      }
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay11.txt").ToList();
      var numbers = lines[0].Split(' ').Select(long.Parse);

      Console.WriteLine(numbers.Select(n => GetNumberOfRocks(n, 75)).Sum());
    }

    private static long GetNumberOfRocks(long number, int iterationsLeft)
    {
      if(iterationsLeft == 0)
      {
        return 1;
      }

      if (!IterationCache.TryGetValue(number, out var innerDictionary))
      {
        innerDictionary = new Dictionary<int, long>();
        IterationCache[number] = innerDictionary;
      }

      if (innerDictionary.TryGetValue(iterationsLeft, out var numberOfRocks))
      {
        return numberOfRocks;
      }

      var totalRocks = 0L;
      var ns = number.ToString();

      if(number == 0)
      {
        totalRocks = GetNumberOfRocks(1, iterationsLeft - 1);
      }
      else if(ns.Length % 2 == 0)
      {
        var firstNumber = long.Parse(string.Join("", ns.Take(ns.Length / 2)));
        var secondNumber = long.Parse(string.Join("", ns.Skip(ns.Length / 2)));
        totalRocks += GetNumberOfRocks(firstNumber, iterationsLeft - 1);
        totalRocks += GetNumberOfRocks(secondNumber, iterationsLeft - 1);
      }
      else
      {
        totalRocks = GetNumberOfRocks(number * 2024, iterationsLeft - 1);
      }

      IterationCache[number][iterationsLeft] = totalRocks;
      return totalRocks;
    }



    // [rock number][times iterated] = value
    private static Dictionary<long, Dictionary<int, long>> IterationCache = new Dictionary<long, Dictionary<int, long>>();
  }
}
