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
  public static class Day9
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay9.txt").ToList();

      Console.WriteLine(lines.Select(l => GetNextValue(l.Split(' ').Select(int.Parse).ToList())).Sum());
    }

    public static int GetNextValue(List<int> values)
    {
      if(values.Distinct().Count() == 1)
      {
        return values[0];
      }

      var diffList = Enumerable.Range(1, values.Count - 1).Select(index => values[index] - values[index - 1]).ToList();
      var diffListValue = GetNextValue(diffList);
      return values.Last() + diffListValue;
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay9.txt").ToList();

      Console.WriteLine(lines.Select(l => GetPreviousValue(l.Split(' ').Select(int.Parse).ToList())).Sum());
    }

    public static int GetPreviousValue(List<int> values)
    {
      if (values.Distinct().Count() == 1)
      {
        return values[0];
      }

      var diffList = Enumerable.Range(1, values.Count - 1).Select(index => values[index] - values[index - 1]).ToList();
      var diffListValue = GetPreviousValue(diffList);
      return values.First() - diffListValue;
    }
  }
}
