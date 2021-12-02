using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
  public static class Day2
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay2.txt").ToList();
      var commands = lines.Select(l => l.Split(' '));
      var forward = commands.Where(l => l[0] == "forward").Sum(l => int.Parse(l[1]));
      var depth = commands.Where(l => l[0] == "down").Sum(l => int.Parse(l[1])) - commands.Where(l => l[0] == "up").Sum(l => int.Parse(l[1]));
      Console.WriteLine(forward * depth);
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay2.txt").ToList();
      var commands = lines.Select(l => l.Split(' '));
      var aim = 0;
      var horizontal = 0;
      var depth = 0;
      foreach (var c in commands)
      {
        var value = int.Parse(c[1]);
        switch (c[0])
        {
          case "forward":
            horizontal += value;
            depth += aim * value;
            break;
          case "down":
            aim = aim + value;
            break;
          case "up":
            aim = aim - value;
            break;

        }
      }
      Console.WriteLine(horizontal * depth);
    }
  }
}
