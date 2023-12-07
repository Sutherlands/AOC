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
  public static class Day6
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay6.txt").ToList();

      var times = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToList();
      var distance = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToList();
      var value = 1;

      for (int x = 0; x < times.Count; ++x)
      {
        var totalWays = 0;
        for (int time = 0; time < times[x]; ++time)
        {
          if (time * (times[x] - time) > distance[x])
            totalWays++;
        }
        value *= totalWays;
      }

      Console.WriteLine(value);
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay6.txt").ToList();

      var times = long.Parse(string.Join("", lines[0].Select(l => char.IsDigit(l) ? l.ToString() : "")));
      var distance = long.Parse(string.Join("", lines[1].Select(l => char.IsDigit(l) ? l.ToString() : "")));

      for (int time = 0; time < times; ++time)
      {
        if (time * (times - time) > distance)
        {
          Console.WriteLine(times-time-time+1);
          return;
        }
      }
    }

  }
}
