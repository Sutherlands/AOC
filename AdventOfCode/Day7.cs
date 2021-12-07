using System;
using System.Collections.Generic;
using System.Diagnostics;
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
      var sw = Stopwatch.StartNew();
      var positions = lines[0].Split(',').Select(int.Parse).GroupBy(p => p).ToDictionary(g => g.Key, g => g.Count());
      var maxPosition = positions.Keys.Max();

      var lowerCache = new int[maxPosition+1];
      var upperCache = new int[maxPosition+1];

      var triangleDistance = 0;
      var numberOfCrabs = 0;
      for(int index = 0; index <= maxPosition; ++index)
      {
        triangleDistance += numberOfCrabs;

        lowerCache[index] = (index == 0 ? 0 : lowerCache[index - 1]) + triangleDistance;

        positions.TryGetValue(index, out var newCrabs);
        numberOfCrabs += newCrabs;
      }

      triangleDistance = 0;
      numberOfCrabs = 0;
      for(int index = maxPosition; index >= 0; --index)
      {
        triangleDistance += numberOfCrabs;

        upperCache[index] = (index == maxPosition ? 0 : upperCache[index + 1]) + triangleDistance;

        positions.TryGetValue(index, out var newCrabs);
        numberOfCrabs += newCrabs;
      }

      var targetX = -1L;
      var leastFuel = long.MaxValue;

      for (int x = 0; x <= maxPosition; ++x)
      {
        var thisFuel = lowerCache[x] + upperCache[x];
        if (thisFuel < leastFuel)
        {
          leastFuel = thisFuel;
          targetX = x;
        }
      }
      var ms = sw.ElapsedMilliseconds;
      Console.WriteLine(ms);
      Console.WriteLine(leastFuel);
    }
  }
}
