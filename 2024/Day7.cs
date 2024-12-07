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
  public static class Day7
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay7.txt").ToList();
      var sum = 0L;
      foreach(var line in lines)
      {
        var parts = line.Split(": ");
        var target = long.Parse(parts[0]);
        var numbers = parts[1].Split(" ").Select(long.Parse).ToList();

        var firstNumber = numbers[0];
        var remaining = numbers.Skip(1).ToList();

        if(IsPossible(firstNumber, remaining, target))
        {
          sum += target;
        }
      }

      Console.WriteLine(sum);
    }

    public static bool IsPossible(long total, List<long> numbers, long target)
    {
      var firstNumber = numbers[0];
      var remaining = numbers.Skip(1).ToList();
      if (numbers.Count == 1)
      {
        return total * firstNumber == target || total + firstNumber == target;
      }

      return IsPossible(firstNumber + total, remaining, target) || IsPossible(firstNumber * total, remaining, target);
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay7.txt").ToList();
      var sum = 0L;
      foreach (var line in lines)
      {
        var parts = line.Split(": ");
        var target = long.Parse(parts[0]);
        var numbers = parts[1].Split(" ").Select(long.Parse).ToList();

        var firstNumber = numbers[0];
        var remaining = numbers.Skip(1).ToList();

        if (IsPossible2(firstNumber, remaining, target))
        {
          sum += target;
        }
      }

      Console.WriteLine(sum);
    }

    public static bool IsPossible2(long total, List<long> numbers, long target)
    {
      var firstNumber = numbers[0];
      var remaining = numbers.Skip(1).ToList();
      if (numbers.Count == 1)
      {
        return total * firstNumber == target || total + firstNumber == target || long.Parse(total.ToString() + firstNumber.ToString()) == target;
      }

      return IsPossible2(firstNumber + total, remaining, target) || IsPossible2(firstNumber * total, remaining, target) || IsPossible2(long.Parse(total.ToString() + firstNumber.ToString()), remaining, target);
    }
  }
}
