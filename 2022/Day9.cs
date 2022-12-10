using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
  public static class Day9
  {
    public static void RunPart1()
    {
      var sw = Stopwatch.StartNew();
      var lines = File.ReadAllLines("./PuzzleInputDay9.txt").ToList();

      var head = new Point(0, 0);
      var tail = new Point(0, 0);
      var visitedPositions = new HashSet<Point>();
      visitedPositions.Add(new Point(0, 0));

      foreach(var line in lines)
      {
        var split = line.Split(" ");
        var direction = split[0];
        var amount = int.Parse(split[1]);

        for(int x = 0; x < amount; ++x)
        {
          switch (direction)
          {
            case "R":
              head.X++;
              break;
            case "L":
              head.X--;
              break;
            case "U":
              head.Y++;
              break;
            case "D":
              head.Y--;
              break;
          }
          UpdatePosition(head, tail);
          visitedPositions.Add(new Point(tail.X, tail.Y));
        }

      }



      Console.WriteLine(visitedPositions.Count());
    }

    private static void UpdatePosition(Point head, Point tail)
    {
      var xDiff = head.X - tail.X;
      var yDiff = head.Y - tail.Y;

      if (xDiff > 1 || yDiff > 1 || xDiff < -1 || yDiff < -1)
      {
        if (xDiff != 0)
        {
          if (xDiff > 0)
          {
            tail.X = tail.X + 1;
          }
          else
          {
            tail.X = tail.X - 1;
          }
        }

        if (yDiff != 0)
        {
          if (yDiff > 0)
          {
            tail.Y = tail.Y + 1;
          }
          else
          {
            tail.Y = tail.Y - 1;
          }
        }

      }
    }

    public static void RunPart2()
    {
      var sw = Stopwatch.StartNew();
      var lines = File.ReadAllLines("./PuzzleInputDay9.txt").ToList();

      var points = new List<Point>();
      for(int x = 0; x < 10; ++x)
      {
        points.Add(new Point(0, 0));
      }
      var head = points[0];

      var visitedPositions = new HashSet<Point>();
      visitedPositions.Add(new Point(0, 0));

      foreach (var line in lines)
      {
        var split = line.Split(" ");
        var direction = split[0];
        var amount = int.Parse(split[1]);

        for (int x = 0; x < amount; ++x)
        {
          switch (direction)
          {
            case "R":
              head.X = head.X + 1;
              break;
            case "L":
              head.X = head.X - 1;
              break;
            case "U":
              head.Y = head.Y + 1;
              break;
            case "D":
              head.Y = head.Y - 1;
              break;
          }
          for (int pointPos = 0; pointPos + 1 < 10; ++pointPos)
          {
            UpdatePosition(points[pointPos], points[pointPos + 1]);
          }
          if(!visitedPositions.Any(p => p.X == points[9].X && p.Y == points[9].Y))
          {
            visitedPositions.Add(new Point(points[9].X, points[9].Y));
          }
        }

      }



      Console.WriteLine(visitedPositions.Count());
    }

    public class Point
    {
      public int X { get; set; }
      public int Y { get; set; }

      public Point(int x, int y)
      {
        X = x;
        Y = y;
      }

      public override string ToString()
      {
        return $"({X},{Y})";
      }
    }

  }
}
