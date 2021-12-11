using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
  public static class Day11
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay11.txt").ToList();
      var grid = new Grid(lines);

      for(int round = 1; round <= 100; ++round)
      {
        grid.StartFlashing(round);
        grid.StopFlashing();

      }
      Console.WriteLine(grid.TotalFlashes);
    }


    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay11.txt").ToList();
      var grid = new Grid(lines);

      for (int round = 1; round <= 10000; ++round)
      {
        grid.StartFlashing(round);
        var total = grid.StopFlashing();
        if (total == 100)
        {
          Console.WriteLine(round);
          return;
        }
      }
      Console.WriteLine(grid.TotalFlashes);
    }

    public class Grid
    {
      private Octopus[] Octopi { get; set; }
      private int Size = 10;
      public int TotalFlashes { get; set; } = 0;

      public Grid(IEnumerable<string> lines)
      {
        Octopi = lines.SelectMany(l => l).Select(l => new Octopus { Luminescence = int.Parse(string.Concat(l)) }).ToArray();
      }

      public Octopus Get(int x, int y)
      {
        return Octopi[x + y * Size];
      }

      public void StartFlashing(int round)
      {
        for(int x = 0; x < 10; ++x)
        {
          for(int y = 0; y < 10; ++y)
          {
            ProcessFlashing(x, y, round);
          }
        }
      }

      public int StopFlashing()
      {
        var total = 0;
        foreach(var o in Octopi)
        {
          if(o.Luminescence > 9)
          {
            o.Luminescence = 0;
            total++;
          }
        }
        return total;
      }

      private void ProcessFlashing(int x, int y, int round)
      {
        if(x < 0 || y < 0 || x >= 10 || y >= 10)
        {
          return;
        }

        var o = Get(x, y);
        o.Luminescence++;

        if(o.Luminescence > 9 && o.LastRoundFlashed < round)
        {
          o.LastRoundFlashed = round;
          TotalFlashes++;

          for(int modX = -1; modX <= 1; ++modX)
          {
            for(int modY = -1; modY <= 1; ++modY)
            {
              if(modX != 0 || modY != 0)
              {
                ProcessFlashing(x + modX, y + modY, round);
              }
            }
          }
        }
      }
    }

    public class Octopus
    {
      public int Luminescence { get; set; }
      public int LastRoundFlashed { get; set; }

    }

  }
}
