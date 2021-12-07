using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
  public static class Day7
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay7.txt").ToList();
      var positions = lines[0].Split(',').Select(long.Parse).ToList();
      var maxPosition = positions.Max();

      var targetX = -1L;
      var leastFuel = long.MaxValue;

      for(long x = 0; x <= maxPosition; ++x)
      {
        var thisFuel = positions.Sum(p => Math.Abs(x - p));
        if(thisFuel < leastFuel)
        {
          leastFuel = thisFuel;
          targetX = x;
        }
      }
      Console.WriteLine(leastFuel);
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay7.txt").ToList();
      var positions = lines[0].Split(',').Select(int.Parse).ToList();
      var maxPosition = positions.Max();

      var targetX = -1L;
      var leastFuel = long.MaxValue;
      var fuelCosts = new Dictionary<int, long>();
      fuelCosts[0] = 0;
      for (int x = 1; x <= maxPosition; ++x)
      {
        fuelCosts[x] = fuelCosts[x - 1] + x;
      }

      for (int x = 0; x <= maxPosition; ++x)
      {
        var thisFuel = positions.Sum(p => fuelCosts[Math.Abs(x - p)]);
        if (thisFuel < leastFuel)
        {
          leastFuel = thisFuel;
          targetX = x;
        }
      }
      Console.WriteLine(leastFuel);
    }
  }
}
