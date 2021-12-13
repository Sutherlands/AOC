using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
  public static class Day13
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay13.txt").ToList();
      var points = lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).Select(l => l.Split(',')).Select(p => new Point(int.Parse(p[0]), int.Parse(p[1]))).ToList();

      var instructions = lines.SkipWhile(l => !string.IsNullOrWhiteSpace(l)).Skip(1).ToArray();

      foreach (var instruction in instructions)
      {
        var fold = instruction.Split(' ').Last();
        var foldInstructions = fold.Split('=');
        points = PerformInstruction(foldInstructions[0], int.Parse(foldInstructions[1]), points);
      }

      Console.WriteLine(points.Distinct().Count());
    }

    public static List<Point> PerformInstruction(string xy, int position, List<Point> points)
    {
      var newPoints = new List<Point>();

      if(xy == "x")
      {
        for(int index = 0; index < points.Count(); ++index)
        {
          var point = points[index];
          if (point.X == position) throw new Exception();
          if(point.X > position)
          {
            var diff = point.X - position;
            point.X = position - diff;
          }
          newPoints.Add(point);
        }
      }
      else
      {
        for (int index = 0; index < points.Count(); ++index)
        {
          var point = points[index];
          if (point.Y == position) throw new Exception();
          if (point.Y > position)
          {
            var diff = point.Y- position;
            point.Y = position - diff;
          }
          newPoints.Add(point);
        }
      }
      return newPoints;
    }

    public static Point PerformSingleInstruction(string xy, int position, Point point)
    {
      if(xy == "x")
      {
        if (point.X > position)
        {
          var diff = point.X - position;
          point.X = position - diff;
        }

        return point;
      }
      else
      {
        if (point.Y > position)
        {
          var diff = point.Y - position;
          point.Y = position - diff;
        }

        return point;
      }
    }

    public static void RunPart2()
    {
      var sw = Stopwatch.StartNew();
      var lines = File.ReadAllLines("./PuzzleInputDay13.txt").ToList();
      var points = lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).Select(l => l.Split(',')).Select(p => new Point(int.Parse(p[0]), int.Parse(p[1]))).ToList();

      var instructions = lines.SkipWhile(l => !string.IsNullOrWhiteSpace(l)).Skip(1).Select(i => i.Split(' ').Last().Split('=')).Select(i => new object[] { i[0], int.Parse(i[1]) }).ToArray();
      //points = points.AsParallel().WithDegreeOfParallelism(500).Select(point =>
      //{
      //  foreach (var instruction in instructions)
      //  {
      //    point = PerformSingleInstruction((string)instruction[0], (int)instruction[1], point);
      //  }
      //  return point;
      //}).ToList();

      foreach (var instruction in instructions)
      {
        points = PerformInstruction((string)instruction[0], (int)instruction[1], points);
      }

      Console.WriteLine(sw.ElapsedMilliseconds);

      var maxX = points.Max(p => p.X);
      var maxY = points.Max(p => p.Y);
      for (int y = 0; y <= maxY; ++y)
      {
        for (int x = 0; x <= maxX; ++x)
        {
          if (points.Contains(new Point(x, y)))
          {
            Console.Write('#');
          }
          else
          {
            Console.Write('.');
          }
        }
        Console.WriteLine();
      }
      Console.WriteLine(points.Distinct().Count());
    }
  }
}
