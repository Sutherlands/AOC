using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
  public static class Day9
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay9.txt").ToList();
      var heightMap = new HeightMap(lines);
      var sumRisk = 0;

      for (int x = 0; x < heightMap.SizeX; ++x)
      {
        for (int y = 0; y < heightMap.SizeY; ++y)
        {
          if (heightMap.IsLowSpot(x, y))
          {
            sumRisk += 1 + heightMap.GetValue(x, y);
          }
        }
      }
      Console.WriteLine(sumRisk);
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay9.txt").ToList();
      var heightMap = new HeightMap(lines);

      HashSet<Point> remainingPoints = new HashSet<Point>();
      for(int x = 0; x < heightMap.SizeX; ++x)
      {
        for(int y = 0; y < heightMap.SizeY; ++y)
        {
          if(heightMap.GetValue(x, y) != 9)
          {
            remainingPoints.Add(new Point(x, y));
          }
        }
      }

      var basinSizes = new List<int>();

      while(remainingPoints.Any())
      {
        var basinSize = GetBasin(remainingPoints);
        basinSizes.Add(basinSize);
      }

      var product = basinSizes.OrderByDescending(s => s).Take(3).Aggregate(1, (a, b) => a*b);

      Console.WriteLine(product);
    }

    private static int GetBasin(HashSet<Point> remainingPoints)
    {
      var startingPoint = remainingPoints.First();
      remainingPoints.Remove(startingPoint);
      var basinSize = 0;

      var pointQueue = new Queue<Point>();
      pointQueue.Enqueue(startingPoint);

      while(pointQueue.Any())
      {
        var currentPoint = pointQueue.Dequeue();
        basinSize++;

        for(int x = -1; x <= 1; ++x)
        {
          for(int y = -1; y <= 1; ++y)
          {
            if(x != 0 && y != 0)
            {
              continue;
            }

            var targetPoint = new Point(currentPoint.X + x, currentPoint.Y + y);
            if (remainingPoints.Contains(targetPoint))
            {
              remainingPoints.Remove(targetPoint);
              pointQueue.Enqueue(targetPoint);
            }
          }
        }
      }

      return basinSize;
    }

    private class HeightMap
    {
      public int SizeX { get; set; }
      public int SizeY { get; set; }
      private int[] Data { get; set; }

      public HeightMap(List<string> lines)
      {
        SizeX = lines[0].Count();
        SizeY = lines.Count();
        Data = lines.SelectMany(l => l.Select(c => c - '0')).ToArray();
      }

      public int GetValue(int x, int y)
      {
        return Data[y * SizeX + x];
      }

      public bool IsLowSpot(int x, int y)
      {
        var currentValue = GetValue(x, y);
        if (x != 0 && (GetValue(x - 1, y) <= currentValue))
        {
          return false;
        }

        if (x != (SizeX-1) && (GetValue(x + 1, y) <= currentValue))
        {
          return false;
        }

        if (y != 0 && (GetValue(x, y - 1) <= currentValue))
        {
          return false;
        }

        if (y != (SizeY-1) && (GetValue(x, y + 1) <= currentValue))
        {
          return false;
        }

        return true;
      }
    }
  }
}
