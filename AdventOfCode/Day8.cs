using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
  public static class Day8
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay8.txt").ToList();
      var finals = lines.Select(l => l.Split('|')[1]).SelectMany(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries));

      var targetLengths = new List<int> { 2, 3, 4, 7 };
      Console.WriteLine(finals.Count(f => targetLengths.Contains(f.Length)));
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay8.txt").ToList();

      var sum = 0;
      foreach(var line in lines)
      {
        var parts = line.Split('|');
        var mapping = GetMapping(parts[0]);
        var outputValue = GetOutput(mapping, parts[1]);
        sum += outputValue;
      }
      
      Console.WriteLine(sum);
    }

    private static int GetOutput(Dictionary<string, int> mapping, string v)
    {
      var digitStrings = v.Split(' ', StringSplitOptions.RemoveEmptyEntries);
      var value = 0;
      foreach(var s in digitStrings)
      {
        value *= 10;
        var orderedString = string.Concat(s.OrderBy(c => c));
        value += mapping[orderedString];
      }
      return value;
    }

    private static Dictionary<string, int> GetMapping(string l)
    {
      var parts = l.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => s.OrderBy(c => c)).ToList();

      var one = parts.Single(p => p.Count() == 2);
      var four = parts.Single(p => p.Count() == 4);
      var seven = parts.Single(p => p.Count() == 3);
      var eight = parts.Single(p => p.Count() == 7);
      
      var top = seven.Except(one).Single();

      var remainingParts = parts.Where(p => p.Count() == 5 || p.Count() == 6).ToList();

      var nine = remainingParts.Single(p => p.Intersect(four).Count() == 4);
      var six = remainingParts.Single(p => p.Count() == 6 && p != nine && p.Intersect(one).Count() == 1);

      var bottomRight = six.Intersect(one).Single();
      var topRight = one.Except(new[] { bottomRight });

      remainingParts = remainingParts.Except(new[] { six, nine }).ToList();

      var zero = remainingParts.Single(p => p.Intersect(six).Count() == 5 && p.Intersect(one).Count() == 2);
      var five = remainingParts.Single(p => p.Intersect(six).Count() == 5 && p.Intersect(one).Count() != 2);

      remainingParts = remainingParts.Except(new[] { zero, five }).ToList();

      var three = remainingParts.Single(p => p.Intersect(one).Count() == 2);
      var two = remainingParts.Single(p => p.Intersect(one).Count() != 2);

      return new Dictionary<string, int> {
        {string.Concat(one), 1 },
        {string.Concat(two), 2 },
        {string.Concat(three), 3 },
        {string.Concat(four), 4 },
        {string.Concat(five), 5 },
        {string.Concat(six), 6 },
        {string.Concat(seven), 7 },
        {string.Concat(eight), 8 },
        {string.Concat(nine), 9 },
        {string.Concat(zero), 0 },
      };
    }
  }
}
