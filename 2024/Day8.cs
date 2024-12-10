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
  public static class Day8
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay8.txt").ToList();
      var pointMap = new Dictionary<char, List<(int x, int y)>>();
      var antinodes = new HashSet<(int x, int y)>();
      for(int x = 0; x < lines[0].Length; ++x)
      {
        for(int y = 0; y < lines.Count; ++y)
        {
          if(lines[y][x] != '.')
          {
            if(!pointMap.ContainsKey(lines[y][x]))
            {
              pointMap[lines[y][x]] = new List<(int x, int y)>();
            }
            pointMap[lines[y][x]].Add((x, y));
          }
        }
      }

      foreach(var pointList in pointMap.Values)
      {
        foreach(var point1 in pointList)
        {
          foreach(var point2 in pointList)
          {
            if (point1 != point2)
            {
              (int x, int y) newPoint = (2 * point1.x - point2.x, 2 * point1.y - point2.y);
              if (newPoint.x >= 0 && newPoint.y >= 0 && newPoint.x < lines[0].Length && newPoint.y < lines.Count)
              {
                antinodes.Add(newPoint);
              }
            }
          }
        }
      }

      Console.WriteLine(antinodes.Count);
    }

    public static void RunPart2()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay8.txt").ToList();
      var pointMap = new Dictionary<char, List<(int x, int y)>>();
      var antinodes = new HashSet<(int x, int y)>();
      for (int x = 0; x < lines[0].Length; ++x)
      {
        for (int y = 0; y < lines.Count; ++y)
        {
          if (lines[y][x] != '.')
          {
            if (!pointMap.ContainsKey(lines[y][x]))
            {
              pointMap[lines[y][x]] = new List<(int x, int y)>();
            }
            pointMap[lines[y][x]].Add((x, y));
          }
        }
      }

      foreach (var pointList in pointMap.Values)
      {
        foreach (var point1 in pointList)
        {
          foreach (var point2 in pointList)
          {
            if (point1 != point2)
            {
              (int x, int y) newPoint = point1;
              while(newPoint.x >= 0 && newPoint.y >= 0 && newPoint.x < lines[0].Length && newPoint.y < lines.Count)
              {
                antinodes.Add(newPoint);
                newPoint.x -= point2.x - point1.x;
                newPoint.y -= point2.y - point1.y;
              }
            }
          }
        }
      }

      Console.WriteLine(antinodes.Count);
    }
  }
}
