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
  public static class Day11
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay11.txt").ToList();
      var galaxyLocations = lines.SelectMany((l, y) => l.Select((c, x) => c == '#' ? new Point(x, y) : new Point(-1, -1))).Where(p => p.X != -1).ToList();

      var pairings = galaxyLocations.SelectMany(galaxyLocation1 => galaxyLocations.Select(galaxyLocation2 => (galaxyLocation1, galaxyLocation2))).ToList();
      var rowValues = Enumerable.Range(0, lines.Count).Select(y => lines[y].Any(c => c == '#') ? 1 : 2).ToList();
      var columnValues = Enumerable.Range(0, lines[0].Length).Select(x => lines.Any(l => l[x] == '#') ? 1 : 2).ToList();

      var totalDistance = 0;

      foreach (var pair in pairings)
      {
        if (pair.galaxyLocation1.Y > pair.galaxyLocation2.Y)
        {
          continue;
        }

        if (pair.galaxyLocation1.Y == pair.galaxyLocation2.Y && pair.galaxyLocation1.X > pair.galaxyLocation2.X)
        {
          continue;
        }

        totalDistance += (int)GetDistance(rowValues, columnValues, pair.galaxyLocation1, pair.galaxyLocation2);
      }

      Console.WriteLine(totalDistance);
    }

    private static long GetDistance(List<int> rowValues, List<int> columnValues, Point galaxyLocation1, Point galaxyLocation2)
    {
      var distance = 0L;
      for (var x = Math.Min(galaxyLocation1.X, galaxyLocation2.X) + 1; x <= galaxyLocation1.X || x <= galaxyLocation2.X; ++x)
      {
        //Console.WriteLine($"Adding x distance {columnValues[x]}");
        distance += columnValues[x];
      }

      for (var y = Math.Min(galaxyLocation1.Y, galaxyLocation2.Y) + 1; y <= galaxyLocation1.Y || y <= galaxyLocation2.Y; ++y)
      {
        //Console.WriteLine($"Adding y distance {rowValues[y]}");
        distance += rowValues[y];
      }

      //Console.WriteLine($"Distance between {galaxyLocation1} and {galaxyLocation2} is {distance}");
      return distance;
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay11.txt").ToList();
      var galaxyLocations = lines.SelectMany((l, y) => l.Select((c, x) => c == '#' ? new Point(x, y) : new Point(-1, -1))).Where(p => p.X != -1).ToList();

      var pairings = galaxyLocations.SelectMany(galaxyLocation1 => galaxyLocations.Select(galaxyLocation2 => (galaxyLocation1, galaxyLocation2))).ToList();
      var rowValues = Enumerable.Range(0, lines.Count).Select(y => lines[y].Any(c => c == '#') ? 1 : 1000000).ToList();
      var columnValues = Enumerable.Range(0, lines[0].Length).Select(x => lines.Any(l => l[x] == '#') ? 1 : 1000000).ToList();

      var totalDistance = 0L;

      foreach (var pair in pairings)
      {
        if (pair.galaxyLocation1.Y > pair.galaxyLocation2.Y)
        {
          continue;
        }

        if (pair.galaxyLocation1.Y == pair.galaxyLocation2.Y && pair.galaxyLocation1.X > pair.galaxyLocation2.X)
        {
          continue;
        }

        totalDistance += GetDistance(rowValues, columnValues, pair.galaxyLocation1, pair.galaxyLocation2);
      }

      Console.WriteLine(totalDistance);
    }
  }
}
