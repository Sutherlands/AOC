using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
  public static class Day5
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay5.txt").ToList();
      var initialSplit = lines.Select(l => l.Split(" -> "));
      var split = lines.Select(l => l.Split(" -> ").SelectMany(lp => lp.Split(',')).Select(int.Parse).ToList());

      var max = split.SelectMany(s => s).Max();
      var container = new PointContainer(max);
      foreach(var entry in split)
      {
        container.AddLine(entry[0], entry[1], entry[2], entry[3]);
      }
      Console.WriteLine(container.GetDangerousPoints());
    }



    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay5.txt").ToList();
      var initialSplit = lines.Select(l => l.Split(" -> "));
      var split = lines.Select(l => l.Split(" -> ").SelectMany(lp => lp.Split(',')).Select(int.Parse).ToList());

      var max = split.SelectMany(s => s).Max();
      var container = new PointContainer(max);
      foreach (var entry in split)
      {
        container.AddLine(entry[0], entry[1], entry[2], entry[3], true);
      }
      Console.WriteLine(container.GetDangerousPoints());
    }

    private class PointContainer
    {
      private int[] points;
      private int size;
      public PointContainer(int maxSize)
      {
        size = maxSize+1;
        points = new int[size * size];
      }

      public void AddLine(int startX, int startY, int endX, int endY, bool includeDiagonals = false)
      {
        int xChange = startX == endX ? 0 : startX < endX ? 1 : -1;
        int yChange = startY == endY ? 0 : startY < endY ? 1 : -1;
        if(xChange != 0 && yChange != 0 && !includeDiagonals)
        {
          return;
        }

        for (int x = startX, y = startY; ;)
        {
          points[GetIndex(x, y)]++;

          if(x == endX && y == endY)
          {
            break;
          }

          x += xChange;
          y += yChange;
        }
      }

      public int GetDangerousPoints()
      {
        return points.Count(p => p > 1);
      }

      private int GetIndex(int x, int y)
      {
        return x * size + y;
      }
    }
  }
}
