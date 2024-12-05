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
  public static class Day5
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay5.txt").ToList();
      var rules = new List<(int before, int after)>();
      var manuals = new List<List<int>>();

      var totalPages = 0;

      foreach(var line in lines)
      {
        if(line.Contains('|'))
        {
          var parts = line.Split('|').Select(int.Parse).ToList();
          rules.Add((parts[0], parts[1]));
        }
        else if(line != "")
        {
          manuals.Add(line.Split(',').Select(int.Parse).ToList());
        }
      }

      foreach(var manual in manuals)
      {
        if(!rules.Any(r =>
        {
          var firstIndex = manual.IndexOf(r.before);
          var secondIndex = manual.IndexOf(r.after);
          return firstIndex != -1 && secondIndex != -1 && firstIndex > secondIndex;
        }))
        {
          totalPages += manual[manual.Count / 2];
        }
      }

      Console.WriteLine(totalPages);
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay5.txt").ToList();
      var rules = new List<(int before, int after)>();
      var manuals = new List<List<int>>();

      var totalPages = 0;

      foreach (var line in lines)
      {
        if (line.Contains('|'))
        {
          var parts = line.Split('|').Select(int.Parse).ToList();
          rules.Add((parts[0], parts[1]));
        }
        else if (line != "")
        {
          manuals.Add(line.Split(',').Select(int.Parse).ToList());
        }
      }

      var sorter = new PageComparer(rules);

      foreach (var manual in manuals)
      {
        if (rules.Any(r =>
        {
          var firstIndex = manual.IndexOf(r.before);
          var secondIndex = manual.IndexOf(r.after);
          return firstIndex != -1 && secondIndex != -1 && firstIndex > secondIndex;
        }))
        {
          manual.Sort(sorter);
          totalPages += manual[manual.Count / 2];
        }
      }

      Console.WriteLine(totalPages);
    }

    public class PageComparer : IComparer<int>
    {
      private readonly List<(int before, int after)> _rules;

      public PageComparer(List<(int before, int after)> rules)
      {
        _rules = rules;
      }

      public int Compare([AllowNull] int x, [AllowNull] int y)
      {
        if (_rules.Any(r => r.before == x && r.after == y))
        {
          return -1;
        }
        if (_rules.Any(r => r.before == y && r.after == x))
        {
          return 1;
        }
        return 0;
      }
    }
  }
}
