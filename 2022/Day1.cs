using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
  public static class Day1
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay1.txt").ToList();

      var intGroupings = lines.Select(s => string.IsNullOrWhiteSpace(s) ? null : (int?)int.Parse(s));
      var elfTotals = new List<int>();
      var currentElfTotal = 0;
      foreach(var val in intGroupings)
      {
        if(val == null)
        {
          elfTotals.Add(currentElfTotal);
          currentElfTotal = 0;
        }
        else
        {
          currentElfTotal += (int)val;
        }
      }
      elfTotals.Add(currentElfTotal);

      Console.WriteLine(elfTotals.Max());
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay1.txt").ToList();

      var intGroupings = lines.Select(s => string.IsNullOrWhiteSpace(s) ? null : (int?)int.Parse(s));
      var elfTotals = new List<int>();
      var currentElfTotal = 0;
      foreach (var val in intGroupings)
      {
        if (val == null)
        {
          elfTotals.Add(currentElfTotal);
          currentElfTotal = 0;
        }
        else
        {
          currentElfTotal += (int)val;
        }
      }
      elfTotals.Add(currentElfTotal);

      
      Console.WriteLine(elfTotals.OrderByDescending(k => k).Take(3).Sum());
    }
  }
}
