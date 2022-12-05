using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
  public static class Day4

  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay4.txt").ToList();
      var total = 0;
      foreach(var line in lines)
      {
        var parts = line.Split(",-".ToCharArray()).Select(int.Parse).ToList();

        if ((parts[0] <= parts[2] && parts[1] >= parts[3]) || (parts[0] >= parts[2] && parts[1] <= parts[3]))
        {
          total++;
        }
      }
      Console.WriteLine(total);
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay4.txt").ToList();
      var total = 0;
      foreach (var line in lines)
      {
        var parts = line.Split(",-".ToCharArray()).Select(int.Parse).ToList();

        if ((parts[0] <= parts[2] && parts[1] >= parts[2]) || (parts[0] <= parts[3] && parts[1] >= parts[3]) || (parts[0] >= parts[2] && parts[1] <= parts[3]))
        {
          total++;
        }
      }
      Console.WriteLine(total);
    }
  }
}
