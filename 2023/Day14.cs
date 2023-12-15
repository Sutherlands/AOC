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
  public static class Day14
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay14.txt").Select(l => l.ToList()).ToList();

      var total = 0;
      for (int x = 0; x < lines[0].Count; ++x)
      {
        for (int y = 0; y < lines.Count; ++y)
        {
          if (lines[y][x] == 'O')
          {
            for (int newY = y; newY >= 0; --newY)
            {
              if (newY == 0 || lines[newY - 1][x] != '.')
              {
                lines[y][x] = '.';
                lines[newY][x] = 'O';
                total += lines.Count - newY;
                break;
              }
            }
          }
        }
      }

      for (int y = 0; y < lines.Count; ++y)
      {
        for (int x = 0; x < lines[0].Count; ++x)
        {
          Console.Write(lines[y][x]);
        }
        Console.WriteLine();
      }
      Console.WriteLine(total);
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay14.txt").Select(l => l.ToArray()).ToArray();

      var cache = new Dictionary<string, (char[][] lines, int counter)>();
      var hits = 0;
      var totalRuns = 1000000000;

      for (int count = 0; count < totalRuns; ++ count)
      {
        var cacheKey = string.Join("", lines.SelectMany(l => l));
        if(cache.ContainsKey(cacheKey))
        {
          hits++;
          lines = cache[cacheKey].lines;
          var loopValue = count - cache[cacheKey].counter;

          while(count + loopValue < totalRuns)
          {
            count += loopValue;
          }
          continue;
        }

        ShiftNorth(lines);
        ShiftWest(lines);
        ShiftSouth(lines);
        ShiftEast(lines);

        cache[cacheKey] = (lines.Select(l => l.ToArray()).ToArray(), count);
      }


      for (int y = 0; y < lines.Length; ++y)
      {
        for (int x = 0; x < lines[0].Length; ++x)
        {
          Console.Write(lines[y][x]);
        }
        Console.WriteLine();
      }
      Console.WriteLine();
      Console.WriteLine($"{hits}/{totalRuns}");

      var total = 0;
      for (int y = 0; y < lines.Length; ++y)
      {
        for (int x = 0; x < lines[0].Length; ++x)
        {
          if (lines[y][x] == 'O')
          {
            total += lines.Length - y;
          }
        }
      }
      Console.WriteLine(total);
    }

    public enum Direction
    {
      North, South, East, West
    }

    public static void ShiftNorth(char[][] lines)
    {
      for (int x = 0; x < lines[0].Length; ++x)
      {
        for (int y = 0; y < lines.Length; ++y)
        {
          if (lines[y][x] == 'O')
          {
            for (int newY = y; newY >= 0; --newY)
            {
              if (newY == 0 || lines[newY - 1][x] != '.')
              {
                lines[y][x] = '.';
                lines[newY][x] = 'O';
                break;
              }
            }
          }
        }
      }
    }

    public static void ShiftSouth(char[][] lines)
    {
      for (int x = 0; x < lines[0].Length; ++x)
      {
        for (int y = lines.Length - 1; y >= 0; --y)
        {
          if (lines[y][x] == 'O')
          {
            for (int newY = y; newY < lines.Length; ++newY)
            {
              if (newY == lines.Length - 1 || lines[newY + 1][x] != '.')
              {
                lines[y][x] = '.';
                lines[newY][x] = 'O';
                break;
              }
            }
          }
        }
      }
    }

    public static void ShiftWest(char[][] lines)
    {
      for (int y = 0; y < lines.Length; ++y)
      {
        for (int x = 0; x < lines[0].Length; ++x)
        {
          if (lines[y][x] == 'O')
          {
            for (int newX = x; newX >= 0; --newX)
            {
              if (newX == 0 || lines[y][newX - 1] != '.')
              {
                lines[y][x] = '.';
                lines[y][newX] = 'O';
                break;
              }
            }
          }
        }
      }
    }

    public static void ShiftEast(char[][] lines)
    {
      for (int y = 0; y < lines.Length; ++y)
      {
        for (int x = lines[0].Length - 1; x >= 0; --x)
        {
          if (lines[y][x] == 'O')
          {
            for (int newX = x; newX < lines[0].Length; ++newX)
            {
              if (newX == lines[0].Length - 1 || lines[y][newX + 1] != '.')
              {
                lines[y][x] = '.';
                lines[y][newX] = 'O';
                break;
              }
            }
          }
        }
      }
    }
  }
}
