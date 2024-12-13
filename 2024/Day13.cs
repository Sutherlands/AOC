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
  public static class Day13
  {
    private static Regex ButtonRegex = new Regex(@"Button .: X\+(\d*), Y\+(\d*)");
    private static Regex LocationRegex = new Regex(@"Prize: X=(\d*), Y=(\d*)");

    public static void RunPart1()
    {
      var lines = File.ReadAllLines("./PuzzleInputDay13.txt").ToList();
      var totalCost = 0L;

      for (int x = 0; x < lines.Count; x += 4)
      {
        var buttonAMatch = ButtonRegex.Match(lines[x]);
        var buttonBMatch = ButtonRegex.Match(lines[x + 1]);
        var locationMatch = LocationRegex.Match(lines[x + 2]);

        (int x, int y) buttonAVector = (int.Parse(buttonAMatch.Groups[1].Value), int.Parse(buttonAMatch.Groups[2].Value));
        (int x, int y) buttonBVector = (int.Parse(buttonBMatch.Groups[1].Value), int.Parse(buttonBMatch.Groups[2].Value));
        (int x, int y) location = (int.Parse(locationMatch.Groups[1].Value), int.Parse(locationMatch.Groups[2].Value));

        var minCost = int.MaxValue;

        for (int aVectorCount = 0; aVectorCount < 100; ++aVectorCount)
        {
          var xDistance = buttonAVector.x * aVectorCount;
          if (xDistance > location.x)
          {
            break;
          }

          var bVectorCount = (location.x - xDistance) / buttonBVector.x;
          if ((bVectorCount * buttonBVector.x + aVectorCount * buttonAVector.x == location.x)
            && (bVectorCount * buttonBVector.y + aVectorCount * buttonAVector.y == location.y))
          {
            var solutionCost = 3 * aVectorCount + bVectorCount;
            minCost = Math.Min(minCost, solutionCost);

            Console.WriteLine(aVectorCount + " " + bVectorCount);
          }
        }

        if (minCost != int.MaxValue)
        {
          totalCost += minCost;
        }
      }

      Console.WriteLine(totalCost);
    }

    public static void RunPart2()
    {
      //80 40
      //38 86
      //480

      var lines = File.ReadAllLines("./PuzzleInputDay13.txt").ToList();
      var totalCost = 0L;

      for (int x = 0; x < lines.Count; x += 4)
      {
        var buttonAMatch = ButtonRegex.Match(lines[x]);
        var buttonBMatch = ButtonRegex.Match(lines[x + 1]);
        var locationMatch = LocationRegex.Match(lines[x + 2]);

        (long x, long y) buttonAVector = (int.Parse(buttonAMatch.Groups[1].Value), int.Parse(buttonAMatch.Groups[2].Value));
        (long x, long y) buttonBVector = (int.Parse(buttonBMatch.Groups[1].Value), int.Parse(buttonBMatch.Groups[2].Value));
        (long x, long y) location = (int.Parse(locationMatch.Groups[1].Value) + 10000000000000, int.Parse(locationMatch.Groups[2].Value) + 10000000000000);


        //b = (position.Y*aVector.X - position.X*aVector.Y ) / (bVector.Y*aVector.X-bVector.X*aVector.Y)

        var bVectorCount = (location.y * buttonAVector.x - location.x * buttonAVector.y) / (buttonBVector.y * buttonAVector.x - buttonBVector.x * buttonAVector.y);

        var xDistance = buttonBVector.x * bVectorCount;
        var aVectorCount = (location.x - xDistance) / buttonAVector.x;

        if(aVectorCount < 0 || bVectorCount < 0 || (bVectorCount * buttonBVector.x + aVectorCount * buttonAVector.x != location.x) || (bVectorCount * buttonBVector.y + aVectorCount * buttonAVector.y != location.y))
        {
          continue;
        }

        Console.WriteLine(aVectorCount + " " + bVectorCount);

        var solutionCost = 3 * aVectorCount + bVectorCount;

        totalCost += solutionCost;
      }

      Console.WriteLine(totalCost);
    }
  }
}
