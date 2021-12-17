using DotNetty.Common.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode
{
  public static class Day17
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay17.txt").ToList();
      var parts = lines[0].Split(',');
      var targetX = getTargetCoordinateRange(parts[0]);
      var targetY = getTargetCoordinateRange(parts[1]);

      var maxY = -targetY.Start;
      var xsThatHit = GetAllowableXValues(targetX);
      for(int y = maxY; true; y--)
      {
        foreach(var x in xsThatHit)
        {
          if (DoesHitRange(x, y, targetX, targetY))
          {
            Console.WriteLine(GetMaxAltitude(y));
            return;
          }
        }
      }
    }

    private static int GetMaxAltitude(int y)
    {
      return (y + 1) * y / 2;
    }

    private static bool DoesHitRange(int x, int y, Range targetX, Range targetY)
    {
      var currentX = 0;
      var currentY = 0;
      while(true)
      {
        currentX += x;
        if (x > 0)
        {
          x--;
        }
        currentY += y;
        y--;

        if(targetX.Start <= currentX && targetX.End >= currentX
          && targetY.Start <= currentY && targetY.End >= currentY)
        {
          return true;
        }

        if(x == 0 && currentY < targetY.Start)
        {
          return false;
        }
      }
    }

    private static List<int> GetAllowableXValues(Range targetX)
    {
      int maxX = Math.Max(targetX.End, 0);
      int minX = Math.Min(targetX.Start, 0);

      return Enumerable.Range(minX, maxX - minX + 1).Where(x => DoesXHitRange(x, targetX)).ToList();
    }

    private static bool DoesXHitRange(int x, Range target)
    {
      var currentX = 0;

      while(x != 0 && currentX < target.End)
      {
        currentX += x;
        if(x > 0)
        {
          x--;
        }
        else
        {
          x++;
        }

        if(target.Start <= currentX && target.End >= currentX)
        {
          return true;
        }
      }
      return false;
    }

    private static Range getTargetCoordinateRange(string text)
    {
      var start = int.Parse(string.Concat(text.SkipWhile(c => !char.IsDigit(c) && !(c == '-')).TakeWhile(c => char.IsDigit(c) || c == '-')));
      var end = int.Parse(string.Concat(text.SkipWhile(c => !char.IsDigit(c)).SkipWhile(c => char.IsDigit(c)).SkipWhile(c => !char.IsDigit(c) && !(c == '-')).TakeWhile(c => char.IsDigit(c) || c == '-')));
      return new Range(start, end);
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay17.txt").ToList();
      var parts = lines[0].Split(',');
      var targetX = getTargetCoordinateRange(parts[0]);
      var targetY = getTargetCoordinateRange(parts[1]);

      var maxY = -targetY.Start;
      var minY = targetY.Start;
      var xsThatHit = GetAllowableXValues(targetX);

      Console.WriteLine(string.Join(", ", xsThatHit));
      var totalCount = 0;
      for (int y = maxY; y >= minY; y--)
      {
        foreach (var x in xsThatHit)
        {
          if (DoesHitRange(x, y, targetX, targetY))
          {
            totalCount++;
          }
        }
      }
      Console.WriteLine(totalCount);
    }

    public class Range
    {
      public Range(int start, int end)
      {
        Start = start;
        End = end;
      }

      public int Start { get; set; }
      public int End { get; set; }
    }
  }
}
