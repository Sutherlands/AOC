using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2024
{
  public static class Day2
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay2.txt").ToList();

      var safeCount = lines.Where(l => IsSafe(l)).Count();
      Console.WriteLine(safeCount);
    }

    private static bool IsSafe(string arg, bool allowError = false)
    {
      var parts = arg.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

      bool isIncreasing = parts[1] > parts[0];
      return ProcessLoop(allowError, parts, isIncreasing) || ProcessLoop(allowError, parts, !isIncreasing);
    }

    private static bool ProcessLoop(bool allowError, List<int> arg, bool isIncreasing)
    {
      var parts = new List<int>(arg);
      var hasError = false;
      for (int index = 1; index < parts.Count; ++index)
      {
        if (isIncreasing ^ parts[index] > parts[index - 1])
        {
          if (!allowError)
          {
            return false;
          }
          else
          {
            hasError = true;
          }
        }

        var difference = Math.Abs(parts[index] - parts[index - 1]);
        if (difference < 1 || difference > 3)
        {
          if (!allowError)
          {
            return false;
          }
          else
          {
            hasError = true;
          }
        }

        if(hasError)
        {
          var duplicate = new List<int>(parts);
          duplicate.RemoveAt(index);
          parts.RemoveAt(index - 1);
          return ProcessLoop(false, duplicate, isIncreasing) || ProcessLoop(false, parts, isIncreasing);
        }
      }
      return true;
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay2.txt").ToList();

      var safeCount = lines.Where(l => IsSafe(l, true)).Count();
      Console.WriteLine(safeCount);
    }
  }
}
