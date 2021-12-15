using DotNetty.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
  public static class Day15
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay15.txt").ToList();
      var sizeX = lines[0].Length;
      var sizeY = lines.Count;

      var heatMap = new int[sizeX, sizeY];
      var pathMap = new int[sizeX, sizeY];

      for(int x = 0; x < sizeX; ++x)
      {
        for(int y = 0; y < sizeY; ++y)
        {
          heatMap[x, y] = lines[x][y] - '0';
        }
      }

      var pQueue = new PriorityQueue<LocationPath>(new PathComparer());
      pQueue.Enqueue(new LocationPath { X = 0, Y = 0, TotalCost = 0 });
      while (true)
      {
        var path = pQueue.Dequeue();
        //Console.WriteLine($"Processing {path.X},{path.Y}.  Remaining: {pQueue.Count}");
        var currentEnd = pathMap[sizeX - 1, sizeY - 1];
        if (path.TotalCost > currentEnd && currentEnd != default)
        {
          break;
        }

        CheckPath(pathMap, heatMap, pQueue, path.X - 1, path.Y, path.TotalCost, sizeX, sizeY);
        CheckPath(pathMap, heatMap, pQueue, path.X + 1, path.Y, path.TotalCost, sizeX, sizeY);
        CheckPath(pathMap, heatMap, pQueue, path.X, path.Y - 1, path.TotalCost, sizeX, sizeY);
        CheckPath(pathMap, heatMap, pQueue, path.X, path.Y + 1, path.TotalCost, sizeX, sizeY);
      }

      Console.WriteLine(pathMap[sizeX-1, sizeY-1]);
    }

    private static void CheckPath(int[,] pathMap, int[,] heatMap, PriorityQueue<LocationPath> pQueue, int x, int y, int previousCost, int maxX, int maxY)
    {
      if(x >= maxX || y >= maxY || x < 0 || y < 0)
      {
        return;
      }
      var existingValue = pathMap[x, y];
      var nodeValue = heatMap[x, y];
      var newValue = nodeValue + previousCost;

      if(existingValue == default || existingValue > newValue)
      {
        pathMap[x, y] = newValue;
        pQueue.Enqueue(new LocationPath { X = x, Y = y, TotalCost = newValue });
      }
    }

    public class PathComparer : IComparer<LocationPath>
    {
      public int Compare([AllowNull] LocationPath x, [AllowNull] LocationPath y)
      {
        return x.TotalCost.CompareTo(y.TotalCost);
      }
    }

    public class LocationPath 
    {
      public int X { get; set; }
      public int Y { get; set; }
      public int TotalCost { get; set; }
    }

    public static void RunPart2()
    {
      var sw = Stopwatch.StartNew();
      var lines = File.ReadAllLines("./PuzzleInputDay15.txt").ToList();
      var sizeX = lines[0].Length;
      var sizeY = lines.Count;

      var heatMap = new int[sizeX*5, sizeY*5];
      var pathMap = new int[sizeX*5, sizeY*5];

      for (int x = 0; x < sizeX; ++x)
      {
        for (int y = 0; y < sizeY; ++y)
        {
          for(int boardModifierX = 0; boardModifierX < 5; ++boardModifierX)
          {
            for(int boardModifierY = 0; boardModifierY < 5; ++boardModifierY)
            {
              var value = (lines[x][y] - '0') + boardModifierX + boardModifierY;
              while(value > 9)
              {
                value -= 9;
              }
              heatMap[x + boardModifierX * sizeX, y + boardModifierY * sizeY] = value;
            }
          }
        }
      }

      int counter = 0;
      var pQueue = new PriorityQueue<LocationPath>(new PathComparer());
      pQueue.Enqueue(new LocationPath { X = 0, Y = 0, TotalCost = 0 });
      while (pQueue.Count > 0)
      {
        counter++;
        var path = pQueue.Dequeue();
        //Console.WriteLine($"Processing {path.X},{path.Y}.  Remaining: {pQueue.Count}");
        var currentEnd = pathMap[sizeX*5 - 1, sizeY*5 - 1];
        if (path.TotalCost > currentEnd && currentEnd != default)
        {
          break;
        }

        CheckPath(pathMap, heatMap, pQueue, path.X - 1, path.Y, path.TotalCost, sizeX * 5, sizeY * 5);
        CheckPath(pathMap, heatMap, pQueue, path.X + 1, path.Y, path.TotalCost, sizeX * 5, sizeY * 5);
        CheckPath(pathMap, heatMap, pQueue, path.X, path.Y - 1, path.TotalCost, sizeX * 5, sizeY * 5);
        CheckPath(pathMap, heatMap, pQueue, path.X, path.Y + 1, path.TotalCost, sizeX * 5, sizeY * 5);
      }

      Console.WriteLine(counter);
      Console.WriteLine(sw.ElapsedMilliseconds);
      Console.WriteLine(pathMap[sizeX*5 - 1, sizeY*5 - 1]);
    }
  }
}
