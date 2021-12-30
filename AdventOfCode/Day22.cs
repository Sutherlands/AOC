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
  public static class Day22
  {
    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay22.txt").ToList();
      var commands = lines.Select(ParseCommand).ToList();

      var pointContainer = new HashSet<Point3D>();
      foreach(var command in commands)
      {
        for(int x = command.XRange.Start; x <= command.XRange.End; ++x)
        {
          for (int y = command.YRange.Start; y <= command.YRange.End; ++y)
          {
            for (int z = command.ZRange.Start; z <= command.ZRange.End; ++z)
            {
              if(x < -50 || y < -50 || z < -50 || x > 50 || y > 50 || z > 50)
              {
                continue;
              }

              var point = new Point3D { X = x, Y = y, Z = z };
              if(command.IsOn)
              {
                pointContainer.Add(point);
              }
              else
              {
                pointContainer.Remove(point);
              }
            }
          }
        }
      }
      Console.WriteLine(pointContainer.Count());
    }

    public static Command ParseCommand(string text)
    {
      var command = new Command();
      var spaceSplit = text.Split(' ');
      command.IsOn = spaceSplit[0] == "on";

      var rangeSplits = spaceSplit[1].Split(',').Select(cs => cs.Split('=')[1]).Select(rs => rs.Split("..")).ToList();
      command.XRange.Start = int.Parse(rangeSplits[0][0]);
      command.XRange.End = int.Parse(rangeSplits[0][1]);
      command.YRange.Start = int.Parse(rangeSplits[1][0]);
      command.YRange.End = int.Parse(rangeSplits[1][1]);
      command.ZRange.Start = int.Parse(rangeSplits[2][0]);
      command.ZRange.End = int.Parse(rangeSplits[2][1]);

      return command;
    }

    public class Command
    {
      public bool IsOn { get; set; }
      public Range XRange { get; set; } = new Range();
      public Range YRange { get; set; } = new Range();
      public Range ZRange { get; set; } = new Range();

      public bool Contains(Point3D point)
      {
        return XRange.Start <= point.X && XRange.End >= point.X
          && YRange.Start <= point.Y && YRange.End >= point.Y
          && ZRange.Start <= point.Z && ZRange.End >= point.Z;
      }
    }

    public class Range
    {
      public int Start { get; set; }
      public int End { get; set; }
    }

    public class Point3D
    {
      public int X { get; set; }
      public int Y { get; set; }
      public int Z { get; set; }

      public override bool Equals(object obj)
      {
        return obj is Point3D d &&
               X == d.X &&
               Y == d.Y &&
               Z == d.Z;
      }

      public override int GetHashCode()
      {
        return HashCode.Combine(X, Y, Z);
      }
    }

    public static void RunPart2()
    {
      var sw = Stopwatch.StartNew();
      var lines = File.ReadAllLines("./PuzzleInputDay22.txt").ToList();
      var commands = lines.Select(ParseCommand).ToList();
      commands.Reverse();

      var xTransitions = GetTransitions(commands.Select(c => c.XRange));
      var yTransitions = GetTransitions(commands.Select(c => c.YRange));
      var zTransitions = GetTransitions(commands.Select(c => c.ZRange));

      var totalArea = 0L;

      for (int xIndex = 0; xIndex < xTransitions.Count - 1; ++xIndex)
      {
        for (int yIndex = 0; yIndex < yTransitions.Count - 1; ++yIndex)
        {
          for (int zIndex = 0; zIndex < zTransitions.Count - 1; ++zIndex)
          {
            var referencePoint = new Point3D
            {
              X = xTransitions[xIndex],
              Y = yTransitions[yIndex],
              Z = zTransitions[zIndex]
            };


            var turnedOn = GetTurnedOn(referencePoint, commands);

            if(turnedOn)
            {
              var area = (long)(xTransitions[xIndex + 1] - xTransitions[xIndex])
                * (long)(yTransitions[yIndex + 1] - yTransitions[yIndex])
                * (long)(zTransitions[zIndex + 1] - zTransitions[zIndex]);
              totalArea += area;
            }
          }
        }
      }

      Console.WriteLine(totalArea);
      Console.WriteLine(sw.ElapsedMilliseconds + "ms");
    }

    private static bool GetTurnedOn(Point3D referencePoint, List<Command> commands)
    {
      foreach(var command in commands)
      {
        if(command.Contains(referencePoint))
        {
          return command.IsOn;
        }
      }

      return false;
    }

    public static List<int> GetTransitions(IEnumerable<Range> ranges)
    {
      return ranges.SelectMany(r => { return new[] { r.Start, r.End + 1 }; })
        .Distinct()
        .OrderBy(x => x)
        .ToList();
    }
  }
}
