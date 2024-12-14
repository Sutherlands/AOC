using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2024
{
  public static class Day14
  {
    private class RobotStats
    {
      public (int x, int y) Position { get; set; }
      public (int x, int y) Velocity { get; set; }

    }

    private static Regex RobotRegex = new Regex(@"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)");

    public static void RunPart1()
    {
      var spaceWidth = 101;
      var spaceHeight = 103;
      var timeToProcess = 100;

      var lines = File.ReadAllLines("./PuzzleInputDay14.txt").ToList();
      var robots = new List<RobotStats>();
      var robotsInQuadrant = new int[] { 0, 0, 0, 0 };
      foreach (var line in lines)
      {
        var match = RobotRegex.Match(line);
        var robot = new RobotStats { Position = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)), Velocity = (int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value)) };
        (int x, int y) newPosition = ((robot.Position.x + robot.Velocity.x * timeToProcess) % spaceWidth, (robot.Position.y + robot.Velocity.y * timeToProcess) % spaceHeight);

        newPosition.x += newPosition.x < 0 ? spaceWidth : 0;
        newPosition.y += newPosition.y < 0 ? spaceHeight : 0;

        if (newPosition.x < spaceWidth / 2)
        {
          if (newPosition.y < spaceHeight / 2)
          {
            robotsInQuadrant[0]++;
          }
          else if (newPosition.y > spaceHeight / 2)
          {
            robotsInQuadrant[1]++;
          }
        }
        else if (newPosition.x > spaceWidth / 2)
        {
          if (newPosition.y < spaceHeight / 2)
          {
            robotsInQuadrant[2]++;
          }
          else if (newPosition.y > spaceHeight / 2)
          {
            robotsInQuadrant[3]++;
          }
        }
      }

      Console.WriteLine(robotsInQuadrant.Aggregate(1, (x, y) => x * y));
    }

    public static void RunPart2()
    {
      var spaceWidth = 101;
      var spaceHeight = 103;

      var lines = File.ReadAllLines("./PuzzleInputDay14.txt").ToList();
      var robots = new List<RobotStats>();
      foreach (var line in lines)
      {
        var match = RobotRegex.Match(line);
        var robot = new RobotStats { Position = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)), Velocity = (int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value)) };
        robots.Add(robot);
      }

      var seconds = 0;
      for(int count = 0; count < spaceWidth * spaceHeight; ++count)
      {
        seconds++;
        foreach (var robot in robots)
        {
          (int x, int y) newPosition = ((robot.Position.x + robot.Velocity.x) % spaceWidth, (robot.Position.y + robot.Velocity.y) % spaceHeight);

          newPosition.x += newPosition.x < 0 ? spaceWidth : 0;
          newPosition.y += newPosition.y < 0 ? spaceHeight : 0;
          robot.Position = newPosition;
        }

        var s = "";
        for (int y = 0; y < spaceHeight; ++y)
        {
          for (int x = 0; x < spaceWidth; ++x)
          {
            s += robots.Any(r => r.Position == (x, y)) ? "*" : " ";
          }
          s += Environment.NewLine;
        }
        
        File.WriteAllText($"{seconds}.txt", s);
        using(ZipArchive zip = ZipFile.Open($"{seconds}.zip", ZipArchiveMode.Create))
        {
          zip.CreateEntryFromFile($"{seconds}.txt", "file.txt");
        }

        if(count % 500 == 0)
        {
          Console.WriteLine(count);
        }
      }
    }
  }
}
