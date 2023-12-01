using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2023
{
  public static class Day1
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay1.txt").ToList();
      var sum = lines.Select(l => l.First(c => char.IsDigit(c)) + "" + l.Last(c => char.IsDigit(c))).Sum(l => int.Parse(l));
      Console.WriteLine(sum);
    }

    static Regex regex = new Regex(@"(?=(one|two|three|four|five|six|seven|eight|nine|\d))");

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay1.txt").ToList();
      lines = lines.Select(line => { var matches = regex.Matches(line); return GetValue(matches.First().Groups[1].Value) + GetValue(matches.Last().Groups[1].Value); }).ToList();
      var sum = lines.Sum(l => int.Parse(l));
      Console.WriteLine(sum);
    }

    public static string GetValue(string text)
    {
      switch (text)
      {
        case "one": return "1";
        case "two": return "2";
        case "three": return "3";
        case "four": return "4";
        case "five": return "5";
        case "six": return "6";
        case "seven": return "7";
        case "eight": return "8";
        case "nine": return "9";
        default:
          return text;
      }
    }

  }
}
