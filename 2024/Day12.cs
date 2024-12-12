using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2024
{
  public static class Day12
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay12.txt").ToList();

      var visitedGrid = lines.Select(l => l.Select(c => false).ToList()).ToList();
      var sum = 0;

      for(int x = 0; x < lines[0].Length; ++x)
      {
        for(int y = 0; y < lines.Count; ++y)
        {
          if(visitedGrid[y][x])
          {
            continue;
          }

          var values = GetValues(lines, visitedGrid, (x, y));
          Console.WriteLine(values.perimeter + " " + values.area);
          sum += values.perimeter * values.area;
        }
      }

      Console.WriteLine(sum);
    }

    private static (int perimeter, int area) GetValues(List<string> lines, List<List<bool>> visitedGrid, (int x, int y) p)
    {
      var area = 0;
      var perimeter = 0;
      var queue = new Queue<(int x, int y)>();
      queue.Enqueue(p);

      while(queue.TryDequeue(out var point))
      {
        if(visitedGrid[point.y][point.x])
        {
          continue;
        }

        visitedGrid[point.y][point.x] = true;
        area++;

        var points = new List<(int x, int y)> { (point.x, point.y + 1), (point.x, point.y - 1), (point.x + 1, point.y), (point.x - 1, point.y) };
        foreach(var sidePoint in points)
        {
          if (IsSame(lines[point.y][point.x], lines, sidePoint))
          {
            queue.Enqueue(sidePoint);
          }
          else
          {
            perimeter++;
          }
        }
      }

      return (perimeter, area);
    }

    private static bool IsSame(char targetSpace, List<string> lines, (int x, int y) p)
    {
      if(p.x < 0 || p.y < 0 || p.x >= lines[0].Length || p.y >= lines.Count)
      {
        return false;
      }
      return targetSpace == lines[p.y][p.x];
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay12.txt").ToList();

      var visitedGrid = lines.Select(l => l.Select(c => false).ToList()).ToList();
      var sum = 0;

      for (int x = 0; x < lines[0].Length; ++x)
      {
        for (int y = 0; y < lines.Count; ++y)
        {
          if (visitedGrid[y][x])
          {
            continue;
          }

          var values = GetValues2(lines, visitedGrid, (x, y));
          Console.WriteLine(values.perimeter + " " + values.area);
          sum += values.perimeter * values.area;
        }
      }

      Console.WriteLine(sum);
    }

    private static (int perimeter, int area) GetValues2(List<string> lines, List<List<bool>> visitedGrid, (int x, int y) startingPoint)
    {
      var area = 0;
      var perimeter = 0;
      var queue = new Queue<(int x, int y)>();
      queue.Enqueue(startingPoint);

      while (queue.TryDequeue(out var currentPoint))
      {
        if (visitedGrid[currentPoint.y][currentPoint.x])
        {
          continue;
        }

        visitedGrid[currentPoint.y][currentPoint.x] = true;
        area++;

        var points = new List<(int x, int y)> { (currentPoint.x, currentPoint.y + 1), (currentPoint.x, currentPoint.y - 1), (currentPoint.x + 1, currentPoint.y), (currentPoint.x - 1, currentPoint.y) };
        foreach (var sidePoint in points)
        {
          if (IsSame(lines[currentPoint.y][currentPoint.x], lines, sidePoint))
          {
            queue.Enqueue(sidePoint);
          }
          else
          {
            if(currentPoint.x != sidePoint.x)
            {
              (int x, int y) aboveTargetPoint = (currentPoint.x, currentPoint.y - 1);
              (int x, int y) aboveOutsidePoint = (sidePoint.x, sidePoint.y - 1);

              if(aboveTargetPoint.y < 0 || !IsSame(lines[currentPoint.y][currentPoint.x], lines, aboveTargetPoint) || IsSame(lines[currentPoint.y][currentPoint.x], lines, aboveOutsidePoint))
              {
                perimeter++;
              }
            }
            else
            {
              (int x, int y) nextToTargetPoint = (currentPoint.x-1, currentPoint.y);
              (int x, int y) nextToOutsidePoint = (sidePoint.x-1, sidePoint.y);

              if (nextToTargetPoint.x < 0 || !IsSame(lines[currentPoint.y][currentPoint.x], lines, nextToTargetPoint) || IsSame(lines[currentPoint.y][currentPoint.x], lines, nextToOutsidePoint))
              {
                perimeter++;
              }
            }
          }
        }
      }

      return (perimeter, area);
    }
  }
}
