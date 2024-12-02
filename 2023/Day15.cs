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
  public static class Day15
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay15.txt").ToList();
      var total = 0;
      foreach (var line in lines[0].Split(","))
      {
        int currentValue = GetHashValue(line);
        total += currentValue;
      }
      Console.WriteLine(total);
    }

    private static int GetHashValue(string line)
    {
      var currentValue = 0;
      foreach (var c in line)
      {
        currentValue += (int)c;
        currentValue *= 17;
        currentValue %= 256;
      }

      return currentValue;
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay15.txt").ToList();
      var boxMap = new Dictionary<int, List<Entry>>();

      foreach (var line in lines[0].Split(","))
      {
        var parts = line.Split('-', '=');
        var label = parts[0];
        int boxValue = GetHashValue(label);
        var operation = line[parts[0].Length];
        var focalLength = operation == '-' ? 0 : int.Parse(parts[1]);
        var box = GetBox(boxMap, boxValue);

        switch (operation)
        {
          case '-':
            box.RemoveAll(e => e.Label == label);
            foreach (var values in box.OrderBy(e => e.Order).Select((e, index) => (e, index)))
            {
              values.e.Order = values.index;
            }
            break;
          case '=':
            var entry = box.FirstOrDefault(e => e.Label == label);
            if (entry != null)
            {
              entry.FocalLength = focalLength;
            }
            else
            {
              box.Add(new Entry { FocalLength = focalLength, Label = label, Order = box.Any() ? box.Max(e => e.Order) + 1 : 0 });
            }
            break;
          default:
            throw new NotImplementedException();
        }
      }

      var total = 0;

      foreach (var entry in boxMap.SelectMany(e => e.Value.Select(Value => (e.Key, Value))))
      {
        var value = (entry.Key + 1) * (entry.Value.Order + 1) * entry.Value.FocalLength;
        total += value;
        Console.WriteLine(value);
      }
      Console.WriteLine(total);
    }

    private static List<Entry> GetBox(Dictionary<int, List<Entry>> boxMap, int boxValue)
    {
      if (boxMap.TryGetValue(boxValue, out var result))
      {
        return result;
      }

      result = new List<Entry>();
      boxMap[boxValue] = result;
      return result;
    }

    public class Entry
    {
      public int Order { get; set; }
      public string Label { get; set; }
      public int FocalLength { get; set; }
    }
  }
}
