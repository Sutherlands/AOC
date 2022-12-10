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

      int headX = 0, headY = 0, tailX = 0, tailY = 0;
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
              headX++;
              break;
            case "L":
              headX--;
              break;
            case "U":
              headY++;
              break;
            case "D":
              headY--;
              break;
          }

          var xDiff = headX - tailX;
          var yDiff = headY - tailY;

          if (xDiff > 1 || yDiff > 1 || xDiff < -1 || yDiff < -1)
          {
            if (xDiff != 0)
            {
              if (xDiff > 0)
              {
                tailX++;
              }
              else
              {
                tailX--;
              }
            }

            if (yDiff != 0)
            {
              if (yDiff > 0)
              {
                tailY++;
              }
              else
              {
                tailY--;
              }
            }
            visitedPositions.Add(new Point(tailX, tailY));
            File.AppendAllText("notmatts.txt", $"({tailX},{tailY}){Environment.NewLine}");

          }

        }

      }



      Console.WriteLine(sw.ElapsedMilliseconds);
      Console.WriteLine(visitedPositions.Count());
    }

    public static void RunPart2()
    {
    }

  }
}
