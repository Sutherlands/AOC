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
  public static class Day3
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay3.txt").ToList();
      var regex = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)");
      var total = 0;

      foreach(var o in regex.Matches(lines[0]))
      {
        var match = (Match)o;
        total += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
      }

      Console.WriteLine(total);
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay3.txt").ToList();
      var regex = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)");
      var total = 0;

      var doIt = true;

      foreach (var o in regex.Matches(lines[0]))
      {
        var match = (Match)o;
        switch(match.Groups[0].Value)
        {
          case "do()":
            doIt = true;
            break;
          case "don't()":
            doIt = false;
            break;
          default:
            if (doIt)
            {
              total += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
            }
            break;
        }
      }

      Console.WriteLine(total);
    }
  }
}
