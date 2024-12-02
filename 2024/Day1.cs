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
  public static class Day1
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay1.txt").ToList();
      var list1 = new List<int>();
      var list2 = new List<int>();

      foreach(var line in lines)
      {
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        list1.Add(int.Parse(parts[0]));
        list2.Add(int.Parse(parts[1]));
      }

      list1.Sort();
      list2.Sort();

      var difference = 0;
      for(int index = 0; index < list1.Count; ++index)
      {
        difference += Math.Abs(list1[index] - list2[index]);
      }

      Console.WriteLine(difference);
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay1.txt").ToList();
      var list1 = new List<int>();
      var list2 = new List<int>();

      foreach (var line in lines)
      {
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        list1.Add(int.Parse(parts[0]));
        list2.Add(int.Parse(parts[1]));
      }

      var counts = list2.GroupBy(l => l).ToDictionary(k => k.Key, v => v.Count());

      var score = 0;
      for (int index = 0; index < list1.Count; ++index)
      {
        counts.TryGetValue(list1[index], out var value);
        score += value * list1[index];
      }

      Console.WriteLine(score);
    }
  }
}
